using Adee.Store.Pays;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Adee.Store.Domain.Pays
{
    public class DefaultPayProvider : IDefaultPayProvider, ITransientDependency
    {
        public virtual Task<PaySuccessResponse> B2C(B2CPayRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual Task<PayResponse> C2B(PayRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual Task<PaySuccessResponse> Query(PayRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual Task<PaySuccessResponse> Refund(RefundPayRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual Task<PaySuccessResponse> RefundQuery(PayRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual Task<PaySuccessResponse> WechatJS(PayRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual Task<AssertNotifyResponse> AssertNotify(AssertNotifyRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TPayResponse> Excute<TPayRequest, TPayResponse>(TPayRequest request)
            where TPayRequest : PayRequest
            where TPayResponse : PayResponse
        {
            var method = GetType().GetMethod(request.PayTaskType.ToString());
            CheckHelper.IsNotNull(method, $"未找到方法：{request.PayTaskType.ToString()}");

            var parameter = method.GetParameters().NullToEmpty().SingleOrDefault();
            CheckHelper.IsNotNull(parameter, $"{request.PayTaskType}方法只能一个参数");
            CheckHelper.AreEqual(parameter.ParameterType.FullName, request.GetType().FullName, message: $"期望传入参数：{parameter.ParameterType.FullName}，实际传入参数：{request.GetType().FullName}");
            CheckHelper.AreEqual(method.ReturnParameter.GetType().FullName, typeof(Task<TPayResponse>).FullName, message: $"期望返回参数：{method.ReturnParameter.GetType().FullName}，实际传入参数：{typeof(Task<TPayResponse>).FullName}");

            return (Task<TPayResponse>)method.Invoke(this, new object[] { request });
        }

        public Task<PayResponse> Cancel(PayRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
