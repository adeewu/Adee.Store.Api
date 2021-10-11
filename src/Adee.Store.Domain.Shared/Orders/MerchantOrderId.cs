using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Volo.Abp;

namespace Adee.Store.Orders
{
    public class MerchantOrderId
    {
        const string DateTimeFormat = "yyyyMMddHHmmssfff";
        const int RandomLength = 3;

        public MerchantOrderId()
        {
            DateTime = DateTime.Now;

            var startRange = $"1{Enumerable.Repeat("0", RandomLength - 1).JoinAsString(string.Empty)}".To<int>();
            var endRange = $"1{Enumerable.Repeat("0", RandomLength).JoinAsString(string.Empty)}".To<int>() - 1;
            Random = new Random(GetHashCode()).Next(startRange, endRange);
        }

        public MerchantOrderId(string MerchantOrderId)
        {
            Check.NotNullOrWhiteSpace(MerchantOrderId, nameof(MerchantOrderId));
            Check.Length(MerchantOrderId, nameof(MerchantOrderId), DateTimeFormat.Length + RandomLength);

            DateTime = DateTime.ParseExact(MerchantOrderId.Substring(0, DateTimeFormat.Length), DateTimeFormat, CultureInfo.InvariantCulture);
            Random = MerchantOrderId.Substring(DateTimeFormat.Length, RandomLength).To<int>();
        }

        public DateTime DateTime { get; private set; }

        public int Random { get; private set; }

        public override string ToString()
        {
            return $"{DateTime:DateTimeFormat}{Random}";
        }
    }
}
