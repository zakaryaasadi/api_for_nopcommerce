using Newtonsoft.Json;
using Nop.Plugin.Api.DTO.Base;

namespace Nop.Plugin.Api.DTOs.ProductReview
{
    [JsonObject(Title = "helpfulness")]
    public class ProductReviewHelpfulnessDto : BaseDto
    {
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("product_review_id")]
        public int ProductReviewId { get; set; }

        [JsonProperty("was_helpful")]
        public bool Washelpful { get; set; }
    }
}
