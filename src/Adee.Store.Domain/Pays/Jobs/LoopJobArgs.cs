namespace Adee.Store.Pays
{
    /// <summary>
    /// 循环任务参数
    /// </summary>
    public class LoopJobArgs
    {
        /// <summary>
        /// 执行次数
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 执行间隔
        /// </summary>
        public int[] Rates { get; set; }
    }
}
