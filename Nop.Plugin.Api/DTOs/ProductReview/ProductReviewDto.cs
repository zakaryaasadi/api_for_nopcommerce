using Newtonsoft.Json;
using Nop.Plugin.Api.DTO.Base;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Api.DTOs.ProductReview
{
    [JsonObject(Title = "product_review")]
    public class ProductReviewDto : BaseDto
    {
        private string _title;
        private string _reviewText;
        private string _replyText;
        private string _customerAvatarUrl;

        public ProductReviewDto()
        {
            CustomerName = string.Empty;
            ReviewTypeMappingsDto = new List<ProductReviewReviewTypeMappingsDto>();
        }

        // <summary>
        /// Gets or sets the title
        /// </summary>
        [JsonProperty("product_id")]
        public int ProductId { get; set; }



        /// <summary>
        /// Gets or sets the customer id
        /// </summary>
        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }


        // <summary>
        /// Gets or sets the title
        /// </summary>
        [JsonProperty("store_id")]
        public int StoreId { get; set; }


        // <summary>
        /// Gets or sets the title
        /// </summary>
        [JsonProperty("title")]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value ?? string.Empty;
            }
        }

        // <summary>
        /// Gets or sets the review text
        /// </summary>
        [JsonProperty("review_text")]
        public string ReviewText
        {
            get
            {
                return _reviewText;
            }
            set
            {
                _reviewText = value ?? string.Empty;
            }
        }

        // <summary>
        /// Gets or sets the reply text
        /// </summary>
        [JsonProperty("reply_text")]
        public string ReplyText
        {
            get
            {
                return _replyText;
            }
            set
            {
                _replyText = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the avatar url for customer
        /// </summary>
        [JsonProperty("customer_avatar_url")]
        public string CustomerAvatarUrl {
            get
            {
                return _customerAvatarUrl;
            }
            set
            {
                _customerAvatarUrl = value ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the customer name
        /// </summary>
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the rating from 1 to 5 stars
        /// </summary>
        [JsonProperty("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets number of customers who liked this review
        /// </summary>
        [JsonProperty("helpful_yes_total")]
        public int HelpfulYesTotal { get; set; }

        /// <summary>
        /// Gets or sets number of customers who did not like this review
        /// </summary>
        [JsonProperty("helpful_no_total")]
        public int HelpfulNoTotal { get; set; }

        /// <summary>
        /// Gets or sets the date and time of review creation
        /// </summary>
        [JsonProperty("created_on_utc")]
        public DateTime CreatedOnUtc { get; set; }


        /// <summary>
        /// Gets or sets the review type mappings
        /// </summary>
        [JsonProperty("review_type_mappings")]
        public List<ProductReviewReviewTypeMappingsDto> ReviewTypeMappingsDto { get; set; }
    }
}
