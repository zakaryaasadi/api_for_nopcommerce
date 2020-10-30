using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Api.DataStructures;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using static Nop.Plugin.Api.Infrastructure.Constants;

namespace Nop.Plugin.Api.Services
{
    public class CategoryApiService : ICategoryApiService
    {
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<ProductCategory> _productCategoryMappingRepository;

        public CategoryApiService(IRepository<Category> categoryRepository,
            IRepository<ProductCategory> productCategoryMappingRepository,
            IStoreMappingService storeMappingService)
        {
            _categoryRepository = categoryRepository;
            _productCategoryMappingRepository = productCategoryMappingRepository;
            _storeMappingService = storeMappingService;
        }

        public IList<Category> GetCategories(IList<int> ids = null, int? ParentCategoryId = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null, DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
            int limit = Configurations.DefaultLimit, int page = Configurations.DefaultPageValue, int sinceId = Configurations.DefaultSinceId, 
            int? productId = null,
            bool? publishedStatus = null, bool? showOnHomePage = null)
        {
            var query = GetCategoriesQuery(ParentCategoryId, createdAtMin, createdAtMax, updatedAtMin, updatedAtMax, publishedStatus, showOnHomePage, productId, ids);


            if (sinceId > 0)
            {
                query = query.Where(c => c.Id > sinceId);
            }

            return new ApiList<Category>(query, page - 1, limit);
        }

        public Category GetCategoryById(int id)
        {
            if (id <= 0)
                return null;

            var category = _categoryRepository.Table.FirstOrDefault(cat => cat.Id == id && !cat.Deleted);

            return category;
        }

        public int GetCategoriesCount(DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
            bool? publishedStatus = null, int? productId = null)
        {
            var query = GetCategoriesQuery(null, createdAtMin, createdAtMax, updatedAtMin, updatedAtMax,
                                           publishedStatus, null, productId);

            return query.Count();
        }

        private IQueryable<Category> GetCategoriesQuery(int? parentCategoryId = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null, DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
            bool? publishedStatus = null, bool? showOnHomePage = null, int? productId = null, IList<int> ids = null)
        {
            var query = _categoryRepository.Table;

            if (ids != null && ids.Count > 0)
            {
                query = query.Where(c => ids.Contains(c.Id));
            }

            if(parentCategoryId != null)
            {
                query = query.Where(c => c.ParentCategoryId == parentCategoryId);
            }

            if (publishedStatus != null)
            {
                query = query.Where(c => c.Published == publishedStatus.Value);
            }

            if (showOnHomePage != null)
            {
                query = query.Where(c => c.ShowOnHomepage == showOnHomePage.Value);
            }

            query = query.Where(c => !c.Deleted);

            if (createdAtMin != null)
            {
                query = query.Where(c => c.CreatedOnUtc > createdAtMin.Value);
            }

            if (createdAtMax != null)
            {
                query = query.Where(c => c.CreatedOnUtc < createdAtMax.Value);
            }

            if (updatedAtMin != null)
            {
                query = query.Where(c => c.UpdatedOnUtc > updatedAtMin.Value);
            }

            if (updatedAtMax != null)
            {
                query = query.Where(c => c.UpdatedOnUtc < updatedAtMax.Value);
            }

            if (productId != null)
            {
                var categoryMappingsForProduct = from productCategoryMapping in _productCategoryMappingRepository.Table
                                                 where productCategoryMapping.ProductId == productId
                                                 select productCategoryMapping;

                query = from category in query
                        join productCategoryMapping in categoryMappingsForProduct on category.Id equals productCategoryMapping.CategoryId
                        select category;
            }

            query = query.OrderBy(category => category.Id);

            return query;
        }
    }
}