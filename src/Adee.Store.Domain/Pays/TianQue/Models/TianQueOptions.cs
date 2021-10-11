namespace Adee.Store.Domain.Pays.TianQue.Models
{
    public class TianQueOptions
    {
        public const string Name = "TianQue";

        /// <summary>
        /// 服务商AppId
        /// </summary>
        public string ServiceAppId { get; set; } = "wx899715cf0859f441";

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; } = "MIICXgIBAAKBgQCjcAC+1MYo/4AXBeqQu5YvmhGkSVCOy/ujYqSR54rzNuj8KhgZXHipma2ePdVTGKXjwUq2KVPwUgQkRzraI+GHJAIGqJTfJvnJ0wefgP7wUbn/s+TIOIcmjidaWsgx7YhkcKKCm1XB2WFpbpW1J4fFfAKlt1PzZWqVb1Wf7sCpIQIDAQABAoGBAJ6IPBDfL0ABZevvLzIo42tVFRu4ic2Zi4NWYa+tWxjEAIbpBetDyT8p9ED0VYJ+/BrKGYBM4kDQLXhLJ4kFGPJ8RImaMZZJlLIgvCGcAxbEQQEwNyLe9qk7PmtxpHsR4tXj63KRAOLHcYGJRvb0Fw14UXSXg2Q4l2a6PlxEygfJAkEA882Q+tJFLChGNKs6Ks+9I6KFVCVQGLO8oNSsG/xSQxRSGHH2R7T2ITcd9l4RpJgrbgR+4+RLZkqPJgq3zxGxLwJBAKudLgbC6eVh6NIhuGHNu+SpzKJB6tayRTNe+fxmCNx0JzGMdbFbJQnGwhRR9WjVITJ5ZF0OjpHfYH5pxJ3Zlq8CQQDwwZlf93FTr9nUfRqN2GOA4yci903ndubZM+taH4vkrhZ8CV4ZZbyBBHrUJgTqM1L/6/Sae4Fx3EMMYB0voNsFAkEAm3kZkpyQ4/PQxlYwQcLuP1mpfzIyu/Djioe3+HbD/lzoiRYUJepJ4tKDT3900lWL7rtVNo0SkosJU10k+FCBFwJAMx0umOGMC+zimJauoaI5yLv0NcLZ6ytTHS8Og/ftbgMuKhIWZTrXC+F+mfsmGXfJZdIlpIHhFNi9p2yeQxZd4Q==";

        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; } = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAjo1+KBcvwDSIo+nMYLeOJ19Ju4ii0xH66ZxFd869EWFWk/EJa3xIA2+4qGf/Ic7m7zi/NHuCnfUtUDmUdP0JfaZiYwn+1Ek7tYAOc1+1GxhzcexSJLyJlR2JLMfEM+rZooW4Ei7q3a8jdTWUNoak/bVPXnLEVLrbIguXABERQ0Ze0X9Fs0y/zkQFg8UjxUN88g2CRfMC6LldHm7UBo+d+WlpOYH7u0OTzoLLiP/04N1cfTgjjtqTBI7qkOGxYs6aBZHG1DJ6WdP+5w+ho91sBTVajsCxAaMoExWQM2ipf/1qGdsWmkZScPflBqg7m0olOD87ymAVP/3Tcbvi34bDfwIDAQAB";

        /// <summary>
        /// 服务商Id
        /// </summary>
        public string OrgId { get; set; } = "32661793";

        /// <summary>
        /// 是否测试环境，默认不是
        /// </summary>
        public bool IsTest { get; set; }
    }
}
