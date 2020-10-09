using Newtonsoft.Json;
using Nop.Plugin.Api.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Api.DTOs.ProductReview
{
    public class ProductReviewsRootObject : ISerializableObject
    {
        public ProductReviewsRootObject()
        {
            ProductReviews = new List<ProductReviewDto>();
        }

        [JsonProperty("product_reviews")]
        public IList<ProductReviewDto> ProductReviews { get; set; }
        public string GetPrimaryPropertyName()
        {
            return "product_reviews";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof(ProductReviewDto);
        }
    }
}
