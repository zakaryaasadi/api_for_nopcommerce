using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.AutoMapper;
using Nop.Plugin.Api.DTOs.ProductReview;

namespace Nop.Plugin.Api.MappingExtensions
{
    public static class ProductReviewDtoMappings
    {
        public static ProductReviewDto ToDto(this ProductReview productReview)
        {
            return productReview.MapTo<ProductReview, ProductReviewDto>();
        }

        public static ProductReviewReviewTypeMappingsDto ToDto(this ProductReviewReviewTypeMapping productReviewReviewTypeMapping)
        {
            return productReviewReviewTypeMapping.MapTo<ProductReviewReviewTypeMapping, ProductReviewReviewTypeMappingsDto>();
        }
    }
}
