using Microsoft.AspNetCore.Http;
using Nop.Plugin.Api.DTOs.ProductReview;
using Nop.Plugin.Api.Helpers;
using System.Collections.Generic;

namespace Nop.Plugin.Api.Validators
{
    public class ProductReviewReviewTypeMappingDtoValidator : BaseDtoValidator<ProductReviewReviewTypeMappingsDto>
    {
        public ProductReviewReviewTypeMappingDtoValidator(IHttpContextAccessor httpContextAccessor, IJsonHelper jsonHelper, Dictionary<string, object> requestJsonDictionary) : base(httpContextAccessor, jsonHelper, requestJsonDictionary)
        {
            SetReviewTypeIdRule();
        }

        private void SetReviewTypeIdRule()
        {
            SetGreaterThanZeroCreateOrUpdateRule(i => i.ReviewTypeId, "invalid review_type_id", "review_type_id");
        }
    }
}
