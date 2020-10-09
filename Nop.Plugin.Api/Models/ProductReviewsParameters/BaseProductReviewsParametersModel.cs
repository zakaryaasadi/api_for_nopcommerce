using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static Nop.Plugin.Api.Infrastructure.Constants;

namespace Nop.Plugin.Api.Models.ProductReviewsParameters
{
    public class BaseProductReviewsParametersModel
    {
        public BaseProductReviewsParametersModel()
        {
            CreatedAtMin = null;
            CreatedAtMax = null;
            ProductId = 0;
            CustomerId = 0;
            SinceId = Configurations.DefaultSinceId;
        }
        /// <summary>
        /// Show products created after date (format: 2008-12-31 03:00)
        /// </summary>
        [JsonProperty("created_at_min")]
        public DateTime? CreatedAtMin { get; set; }

        /// <summary>
        /// Show products created before date (format: 2008-12-31 03:00)
        /// </summary>
        [JsonProperty("created_at_max")]
        public DateTime? CreatedAtMax { get; set; }

        /// <summary>
        /// Show only the reviews mapped to the specified product
        /// </summary>
        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        /// <summary>
        /// Show only the reviews mapped to the specified product
        /// </summary>
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }


        /// <summary>
        /// Restrict results to after the specified ID
        /// </summary>
        [JsonProperty("since_id")]
        public int SinceId { get; set; }
    }
}
