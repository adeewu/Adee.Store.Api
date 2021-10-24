using System;
using System.Collections;
using System.IO;
using System.Net;
//using System.Web;
using System.Security.Cryptography;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Pays.Utils.Helpers
{
    public class WechatCryptHelper : ITransientDependency
    {
        private readonly AESHelper _aesHelper;

        private string _token;
        private string _encodingAESKey;
        private string _appID;

        public WechatCryptHelper(AESHelper aesHelper)
        {
            _aesHelper = aesHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token">公众平台上，开发者设置的Token</param>
        /// <param name="encodingAESKey">公众平台上，开发者设置的EncodingAESKey</param>
        /// <param name="appId">公众帐号的appId</param>
        public void Init(string token, string encodingAESKey, string appId)
        {
            _token = token;
            _appID = appId;
            _encodingAESKey = encodingAESKey;
        }

        /// <summary>
        /// 检验消息的真实性，并且获取解密后的明文
        /// </summary>
        /// <param name="msgSignature">签名串，对应URL参数的msg_signature</param>
        /// <param name="timeStamp">时间戳，对应URL参数的timestamp</param>
        /// <param name="nonce">随机串，对应URL参数的nonce</param>
        /// <param name="encryptMsg">密文，对应POST请求的数据</param>
        /// <returns>解密后的原文</returns>
        public string DecryptMsg(string msgSignature, string timeStamp, string nonce, string encryptMsg)
        {
            CheckHelper.IsTrue(_encodingAESKey.Length == 43, "AESKey 非法");

            //verify signature
            VerifySignature(_token, timeStamp, nonce, encryptMsg, msgSignature);

            //decrypt            
            try
            {
                string appId = "";
                var result = Decrypt(encryptMsg, _encodingAESKey, ref appId);

                CheckHelper.IsTrue(appId == _appID, "appid 校验错误");

                return result;
            }
            catch (FormatException ex)
            {
                throw new Exception("base64解密异常", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("AES 解密失败", ex);
            }
        }

        /// <summary>
        /// 将企业号回复用户的消息加密打包
        /// </summary>
        /// <param name="sReplyMsg">企业号待回复用户的消息，xml格式的字符串</param>
        /// <param name="timeStamp">时间戳，可以自己生成，也可以用URL参数的timestamp</param>
        /// <param name="nonce">加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串,当return返回0时有效</param>
        /// <returns>成功0，失败返回对应的错误码</returns>
        public string EncryptMsg(string sReplyMsg, string timeStamp, string nonce)
        {
            CheckHelper.IsTrue(_encodingAESKey.Length == 43, "AESKey 非法");

            string raw;
            try
            {
                raw = Encrypt(sReplyMsg, _encodingAESKey, _appID);
            }
            catch (Exception ex)
            {
                throw new Exception("AES 加密失败", ex);
            }

            var msgSigature = GenarateSinature(_token, timeStamp, nonce, raw);

            return $"<xml><Encrypt><![CDATA[{raw}]]></Encrypt><MsgSignature><![CDATA[{msgSigature}]]></MsgSignature><TimeStamp><![CDATA[{timeStamp}]]></TimeStamp><Nonce><![CDATA[{nonce}]]></Nonce></xml>";
        }

        public class DictionarySort : IComparer
        {
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;

            }
        }

        private void VerifySignature(string token, string timeStamp, string nonce, string msgEncrypt, string sigture)
        {
            var hash = GenarateSinature(token, timeStamp, nonce, msgEncrypt);
            CheckHelper.IsTrue(hash == sigture, "签名验证错误");
        }

        public string GenarateSinature(string token, string timeStamp, string nonce, string msgEncrypt)
        {
            ArrayList AL = new ArrayList();
            AL.Add(token);
            AL.Add(timeStamp);
            AL.Add(nonce);
            AL.Add(msgEncrypt);
            AL.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }

            try
            {
                var sha = new SHA1CryptoServiceProvider();
                var enc = new ASCIIEncoding();

                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);

                var hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
                return hash;
            }
            catch (Exception ex)
            {
                throw new Exception("sha加密生成签名失败", ex);
            }
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="input">密文</param>
        /// <param name="encodingAESKey"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        /// 
        private string Decrypt(string input, string encodingAESKey, ref string appid)
        {
            byte[] key;
            key = Convert.FromBase64String(encodingAESKey + "=");
            byte[] iv = new byte[16];
            Array.Copy(key, iv, 16);
            byte[] btmpMsg =_aesHelper.Decrypt(input, iv, key);

            int len = BitConverter.ToInt32(btmpMsg, 16);
            len = IPAddress.NetworkToHostOrder(len);

            byte[] bMsg = new byte[len];
            byte[] bAppid = new byte[btmpMsg.Length - 20 - len];
            Array.Copy(btmpMsg, 20, bMsg, 0, len);
            Array.Copy(btmpMsg, 20 + len, bAppid, 0, btmpMsg.Length - 20 - len);
            string oriMsg = Encoding.UTF8.GetString(bMsg);
            appid = Encoding.UTF8.GetString(bAppid);

            return oriMsg;
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodingAESKey"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        private string Encrypt(string input, string encodingAESKey, string appid)
        {
            byte[] Key;
            Key = Convert.FromBase64String(encodingAESKey + "=");
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            string Randcode = CreateRandCode(16);
            byte[] bRand = Encoding.UTF8.GetBytes(Randcode);
            byte[] bAppid = Encoding.UTF8.GetBytes(appid);
            byte[] btmpMsg = Encoding.UTF8.GetBytes(input);
            byte[] bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(btmpMsg.Length));
            byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bAppid.Length + btmpMsg.Length];

            Array.Copy(bRand, bMsg, bRand.Length);
            Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
            Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
            Array.Copy(bAppid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bAppid.Length);

            return _aesHelper.Encrypt(bMsg, Iv, Key);
        }

        private string CreateRandCode(int codeLen)
        {
            string codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
            if (codeLen == 0)
            {
                codeLen = 16;
            }
            string[] arr = codeSerial.Split(',');
            string code = "";
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }

        private int HostToNetworkOrder(int inval)
        {
            int outval = 0;
            for (int i = 0; i < 4; i++)
            {
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            }
            return outval;
        }
    }
}
