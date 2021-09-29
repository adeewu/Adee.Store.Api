using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Adee.Store.Products
{
    /// <summary>
    /// 商品分类
    /// </summary>
    public class ProductCatalogAppService : CrudAppService<ProductCatalog, ProductCatalogDto, Guid, ProductCatalogListDto, CreateUpdateProductCatalogDto>, IProductCatalogAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductCatalogAppService(
            IRepository<ProductCatalog, Guid> productCatalogRepository,
            IRepository<Product, Guid> productRepository)
            : base(productCatalogRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// 获取分类树形集合
        /// </summary>
        /// <param name="parentCatalogId">父类Id，留空为根分类</param>
        /// <param name="isOnlyFirstChild">仅仅第一级节点</param>
        /// <returns></returns>
        public async Task<List<ProductCatalogDto>> GetTreeAsync(Guid parentCatalogId, bool isOnlyFirstChild = true)
        {
            var catalogs = new List<ProductCatalog>();
            if (isOnlyFirstChild)
            {
                var catalogQueryable = Repository
                    .WhereIf(parentCatalogId != Guid.Empty, p => p.ParentCatalogId == parentCatalogId)
                    .WhereIf(parentCatalogId == Guid.Empty, p => !p.ParentCatalogId.HasValue || (p.ParentCatalogId.HasValue && p.ParentCatalogId == Guid.Empty));
                catalogs = await AsyncExecuter.ToListAsync(catalogQueryable);
            }
            else
            {
                var catalogPath = string.Empty;

                if (parentCatalogId != Guid.Empty)
                {
                    var parentCatalog = await Repository.FirstOrDefaultAsync(p => p.Id == parentCatalogId);
                    Check.NotNull(parentCatalog, nameof(parentCatalog));

                    catalogPath = parentCatalog.CatalogPath + "-";
                }

                catalogs = await Repository.GetListAsync(p => p.CatalogPath.StartsWith(catalogPath));
            }

            var catalogDtos = ObjectMapper.Map<List<ProductCatalog>, List<ProductCatalogDto>>(catalogs);

            Action<ProductCatalogDto> action = null;
            action = (currnetCatalog) =>
            {
                var subCatalogs = catalogDtos.Where(p => p.ParentCatalogId == currnetCatalog.Id).ToList();
                if (subCatalogs.Any())
                {
                    currnetCatalog.SubProductCatalogs = subCatalogs;
                    foreach (var item in subCatalogs)
                    {
                        action.Invoke(item);
                    }
                }
                else
                {
                    currnetCatalog.SubProductCatalogs = new List<ProductCatalogDto>();
                }
            };

            var rootCatalogDtos = catalogDtos.Where(p => p.ParentCatalogId == parentCatalogId).ToList();
            foreach (var item in rootCatalogDtos)
            {
                action.Invoke(item);
            }

            return rootCatalogDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<ProductCatalogDto> CreateAsync(CreateUpdateProductCatalogDto input)
        {
            if (input.Name.Contains("-")) throw new UserFriendlyException("分类名称不能包含关键字\"-\"");

            var duplicateName = await Repository.AnyAsync(p => p.Name == input.Name);
            if (duplicateName) throw new UserFriendlyException($"名称:{input.Name}已存在");

            var catalog = new ProductCatalog(GuidGenerator.Create());
            ObjectMapper.Map(input, catalog);

            if (input.ParentCatalogId.HasValue && input.ParentCatalogId != Guid.Empty)
            {
                var parentCatalog = await Repository.FirstOrDefaultAsync(p => p.Id == input.ParentCatalogId);
                Check.NotNull(parentCatalog, nameof(parentCatalog), "父分类不能为空");

                catalog.CatalogPath = parentCatalog.CatalogPath + "-" + catalog.Name;
            }
            else
            {
                catalog.CatalogPath = catalog.Name;
            }

            catalog.TenantId = CurrentTenant.Id;

            await Repository.InsertAsync(catalog, autoSave: true);

            return ObjectMapper.Map<ProductCatalog, ProductCatalogDto>(catalog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<ProductCatalogDto> UpdateAsync(Guid id, CreateUpdateProductCatalogDto input)
        {
            if (input.Name.Contains("-")) throw new UserFriendlyException("分类名称不能包含关键字\"-\"");

            var duplicateName = await Repository.AnyAsync(p => p.Name == input.Name);
            if (duplicateName) throw new UserFriendlyException($"名称:{input.Name}已存在");

            var catalog = await Repository.FirstOrDefaultAsync(p => p.Id == id);
            Check.NotNull(catalog, nameof(catalog), $"分类【{id}】不存在");

            var subCatalogs = await Repository.GetListAsync(p => p.CatalogPath.StartsWith($"{catalog.CatalogPath}-{catalog.Name}-"));

            if (input.ParentCatalogId != catalog.ParentCatalogId)
            {
                var parentCatalog = await Repository.FirstOrDefaultAsync(p => p.Id == input.ParentCatalogId);
                Check.NotNull(parentCatalog, nameof(parentCatalog), "父分类不能为空");

                catalog.ParentCatalogId = input.ParentCatalogId;
                if (catalog.ParentCatalogId.HasValue && catalog.ParentCatalogId != Guid.Empty)
                {
                    catalog.CatalogPath = parentCatalog.CatalogPath + "-" + catalog.Name;
                }
                else
                {
                    catalog.CatalogPath = catalog.Name;
                }
            }

            if (input.Name != catalog.Name)
            {
                foreach (var item in subCatalogs)
                {
                    item.CatalogPath = catalog.CatalogPath + "-" + item.Name;
                    await Repository.UpdateAsync(item);
                }
            }

            ObjectMapper.Map(input, catalog);

            catalog = await Repository.UpdateAsync(catalog, autoSave: true);

            return ObjectMapper.Map<ProductCatalog, ProductCatalogDto>(catalog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task DeleteAsync(Guid id)
        {
            var catalog = await Repository.FirstOrDefaultAsync(p => p.Id == id);
            Check.NotNull(catalog, nameof(catalog), $"分类【{id}】不存在");

            var subCatalogIds = (await Repository.GetListAsync(p => p.CatalogPath.StartsWith($"{catalog.CatalogPath }-"))).NullToEmpty().Select(p => p.Id).ToList();
            if (subCatalogIds.Any()) throw new UserFriendlyException("需要删除子分类后再删除此分类");

            subCatalogIds.Add(id);
            var hasProduct = await _productRepository.AnyAsync(p => subCatalogIds.Contains(p.CatalogId));
            if (hasProduct) throw new UserFriendlyException("需要删除分类下的商品才能删除此分类");

            await base.DeleteAsync(id);
        }
    }
}
