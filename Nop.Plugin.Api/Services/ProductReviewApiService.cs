using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Api.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using static Nop.Plugin.Api.Infrastructure.Constants;

namespace Nop.Plugin.Api.Services
{
    public class ProductReviewApiService : IProductReviewApiService
    {
        private readonly IRepository<ProductReview> _productReviewRepository;
        public ProductReviewApiService(IRepository<ProductReview> productReviewRepository)
        {
            _productReviewRepository = productReviewRepository;
        }
        public IList<ProductReview> GetProductReviews(int customerId = 0,
            int productId = 0, IList<int> ids = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            int limit = Configurations.DefaultLimit, int page = Configurations.DefaultPageValue, 
            int sinceId = Configurations.DefaultSinceId)
        {
            var query = GetProductReviewQuery(customerId, productId, ids, createdAtMin, createdAtMax);

            if (sinceId > 0)
            {
                query = query.Where(pr => pr.Id > sinceId);
            }

            return new ApiList<ProductReview>(query, page - 1, limit);
        }


        public int GetProductReviewsCount(int customerId = 0,
            int productId = 0, 
            DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            int sinceId = Configurations.DefaultSinceId)
        {
            var query = GetProductReviewQuery(customerId, productId, createdAtMin: createdAtMin,  createdAtMax: createdAtMax);
            if (sinceId > 0)
            {
                query = query.Where(pr => pr.Id > sinceId);
            }

            return query.Count();
        }

        public ProductReview GetProductReviewById(int productReviewId)
        {
            if (productReviewId <= 0)
                return null;

            return _productReviewRepository.Table.FirstOrDefault(pr => pr.Id == productReviewId && pr.IsApproved);
        }

        private IQueryable<ProductReview> GetProductReviewQuery(int customerId = 0,
            int productId = 0, IList<int> ids = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null)
        {
            var query = _productReviewRepository.Table;

            query = query.Where(pr => pr.IsApproved);


            if(customerId > 0)
            {
                query = query.Where(pr => pr.CustomerId == customerId);
            }
            if(productId > 0)
            {
                query = query.Where(pr => pr.ProductId == productId);
            }

            if (ids != null && ids.Count > 0)
            {
                query = query.Where(pr => ids.Contains(pr.Id));
            }

            if (createdAtMin != null)
            {
                query = query.Where(pr => pr.CreatedOnUtc > createdAtMin.Value.ToUniversalTime());
            }

            if (createdAtMax != null)
            {
                query = query.Where(pr => pr.CreatedOnUtc < createdAtMax.Value.ToUniversalTime());
            }

            return query;
        }
    }
}
