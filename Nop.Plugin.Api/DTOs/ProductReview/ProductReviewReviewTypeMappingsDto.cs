using Newtonsoft.Json;
using Nop.Plugin.Api.DTO.Base;

namespace Nop.Plugin.Api.DTOs.ProductReview
{
    public class ProductReviewReviewTypeMappingsDto : BaseDto
    {
        // <summary>
        /// Gets or sets the product review id
        /// </summary>
        [JsonProperty("product_review_id")]
        public int ProductReviewId { get; set; }



        /// <summary>
        /// Gets or sets the review type id
        /// </summary>
        [JsonProperty("review_type_id")]
        public int ReviewTypeId { get; set; }


        // <summary>
        /// Gets or sets the ratin
        /// </summary>
        [JsonProperty("rating")]
        public int Rating { get; set; }
    }
}
