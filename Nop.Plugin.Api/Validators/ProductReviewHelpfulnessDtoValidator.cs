using Microsoft.AspNetCore.Http;
using Nop.Plugin.Api.Helpers;
using Nop.Plugin.Api.DTOs.ProductReview;
using System.Collections.Generic;

namespace Nop.Plugin.Api.Validators
{
    public class ProductReviewHelpfulnessDtoValidator : BaseDtoValidator<ProductReviewHelpfulnessDto>
    {
        #region Constructors
        public ProductReviewHelpfulnessDtoValidator(IHttpContextAccessor httpContextAccessor, IJsonHelper jsonHelper, Dictionary<string, object> requestJsonDictionary) : base(httpContextAccessor, jsonHelper, requestJsonDictionary)
        {
            SetCustomerIdRule();
            SetProductReviewIdRule();
        }

        #endregion

        #region Private Methods
        private void SetCustomerIdRule()
        {
            SetGreaterThanZeroCreateOrUpdateRule(c => c.CustomerId, "invalid customer_id", "customer_id");
        }

        private void SetProductReviewIdRule()
        {
            SetGreaterThanZeroCreateOrUpdateRule(c => c.ProductReviewId, "invalid product_review_id", "product_review_id");
        }
        #endregion
    }
}
