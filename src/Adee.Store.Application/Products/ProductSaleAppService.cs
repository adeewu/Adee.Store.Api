using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adee.Store.Attributes;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品上架
    /// </summary>
    [ApiGroup(ApiGroupType.Product)]
    public class ProductSaleAppService : CrudAppService<ProductSale, ProductSaleDto, Guid, ProductSaleListDto, CreateUpdateProductSaleDto>, IProductSaleAppService
    {
        public ProductSaleAppService(
            IRepository<ProductSale, Guid> productSaleRespository)
            : base(productSaleRespository)
        {

        }
    }
}
