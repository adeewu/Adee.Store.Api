namespace Adee.Store.Pays
{
    /// <summary>
    /// 支付结果
    /// </summary>
    public class PayTaskResult
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        public PayTaskStatus Status { get; set; }

        public string Message { get; set; }
    }
}
