using Newtonsoft.Json;
using Nop.Plugin.Api.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Api.DTOs.ProductReview
{
    public class ProductReviewsCountRootObject
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
