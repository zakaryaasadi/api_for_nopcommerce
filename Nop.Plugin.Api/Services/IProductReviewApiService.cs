using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Text;
using static Nop.Plugin.Api.Infrastructure.Constants;

namespace Nop.Plugin.Api.Services
{
    public interface IProductReviewApiService
    {
        IList<ProductReview> GetProductReviews(int customerId = 0, 
            int productId = 0, IList<int> ids = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            int limit = Configurations.DefaultLimit, int page = Configurations.DefaultPageValue, 
            int sinceId = Configurations.DefaultSinceId);

        int GetProductReviewsCount(int customerId = 0, 
            int productId = 0,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            int sinceId = Configurations.DefaultSinceId);

        ProductReview GetProductReviewById(int productReviewId);

    }
}
