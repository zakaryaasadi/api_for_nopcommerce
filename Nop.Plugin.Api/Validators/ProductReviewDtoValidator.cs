using FluentValidation;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.Api.DTOs.ProductReview;
using Nop.Plugin.Api.Helpers;
using System.Collections.Generic;
using System.Net.Http;

namespace Nop.Plugin.Api.Validators
{
    public class ProductReviewDtoValidator : BaseDtoValidator<ProductReviewDto>
    {
        #region Constructors
        public ProductReviewDtoValidator(IHttpContextAccessor httpContextAccessor, IJsonHelper jsonHelper, Dictionary<string, object> requestJsonDictionary) : base(httpContextAccessor, jsonHelper, requestJsonDictionary)
        {
            SetCustomerIdRule();
            SetProductIdRule();

            SetTitleRule();
            SetTextRule();

            SetReviewTypeMappingRule();
        }

        #endregion

        #region Private Methods

        private void SetReviewTypeMappingRule()
        {
            var key = "review_type_mappings";
            if (RequestJsonDictionary.ContainsKey(key))
            {
                RuleForEach(pr => pr.ReviewTypeMappingsDto)
                    .Custom((reviewTypeMappingDto, validationContext) =>
                    {
                        var productTypeMappinDtoJsonDictionary = GetRequestJsonDictionaryCollectionItemDictionary(key, reviewTypeMappingDto);

                        var validator = new ProductReviewReviewTypeMappingDtoValidator(HttpContextAccessor, JsonHelper, productTypeMappinDtoJsonDictionary);

                        //force create validation for new review type mapping 
                        if (reviewTypeMappingDto.Id == 0)
                        {
                            validator.HttpMethod = HttpMethod.Post;
                        }

                        var validationResult = validator.Validate(reviewTypeMappingDto);
                        MergeValidationResult(validationContext, validationResult);
                    });
            }
        }

        private void SetCustomerIdRule()
        {
            SetGreaterThanZeroCreateOrUpdateRule(c => c.CustomerId, "invalid customer_id", "customer_id");
        }

        private void SetProductIdRule()
        {
            SetGreaterThanZeroCreateOrUpdateRule(c => c.ProductId, "invalid product_id", "product_id");
        }



        private void SetTitleRule()
        {
            SetNotNullOrEmptyCreateOrUpdateRule(c => c.Title, "title is required", "title");
        }

        private void SetTextRule()
        {
            SetNotNullOrEmptyCreateOrUpdateRule(c => c.ReviewText, "review_text is required", "review_text");
        }

        #endregion
    }
}
