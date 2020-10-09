using System.Collections.Generic;
using Newtonsoft.Json;
using static Nop.Plugin.Api.Infrastructure.Constants;
using Nop.Plugin.Api.ModelBinders;

namespace Nop.Plugin.Api.Models.ProductsParameters
{
    using Microsoft.AspNetCore.Mvc;

    // JsonProperty is used only for swagger
    [ModelBinder(typeof(ParametersModelBinder<ProductsParametersModel>))]
    public class ProductsParametersModel : BaseProductsParametersModel
    {
        public ProductsParametersModel()
        {
            Ids = null;
            Limit = Configurations.DefaultLimit;
            Page = Configurations.DefaultPageValue;
            SinceId = Configurations.DefaultSinceId;
            Fields = string.Empty;
            ShowOnHomePage = null;
        }

        /// <summary>
        /// A comma-separated list of order ids
        /// </summary>
        [JsonProperty("ids")]
        public List<int> Ids { get; set; }

        /// <summary>
        /// Amount of results (default: 50) (maximum: 250)
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Page to show (default: 1)
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// Restrict results to after the specified ID
        /// </summary>
        [JsonProperty("since_id")]
        public int SinceId { get; set; }

        /// <summary>
        /// comma-separated list of fields to include in the response
        /// </summary>
        [JsonProperty("fields")]
        public string Fields { get; set; }

        /// <summary>
        /// <ul>
        /// <li>yes - Show only show on home page categories</li>
        /// <li>no - Show only not show on home page categories</li>
        /// <li>any - Show all categories(default)</li>
        /// </ul>
        /// </summary>
        [JsonProperty("show_on_home_page")]
        public bool? ShowOnHomePage { get; set; }
    }
}