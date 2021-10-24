using System.IO;
using System.Text;
using System.Xml.Serialization;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Pays.Utils.Helpers
{
    /// <summary>
    /// Xml序列化与反序列化
    /// </summary>
    public class XmlHelper: ISingletonDependency
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Deserialize<T>(string xml)
        {
            var buffer = Encoding.UTF8.GetBytes(xml);

            using (var ms = new MemoryStream(buffer))
            {
                XmlSerializer xmlSearializer = new XmlSerializer(typeof(T), new XmlRootAttribute("xml"));
                T info = (T)xmlSearializer.Deserialize(ms);
                return info;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Deserialize<T>(Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            return (T)xmldes.Deserialize(stream);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string Serializer<T>(T t)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));

                xml.Serialize(Stream, t);
                Stream.Position = 0;
                using (StreamReader sr = new StreamReader(Stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}

