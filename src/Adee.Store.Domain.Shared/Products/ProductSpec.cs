using System;
using System.Collections.Generic;
using System.Text;

namespace Adee.Store.Products
{
    public class ProductSpec
    {
        public ProductSpec()
        {
            SubProductSpecs = new List<ProductSpec>();
        }

        public string Name { get; set; }

        public int Order { get; set; }

        public List<ProductSpec> SubProductSpecs { get; set; }
    }
}
