using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.ModelBinders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Api.Models.ProductsParameters
{
    [ModelBinder(typeof(ParametersModelBinder<SearchProductsParametersModel>))]
    public class SearchProductsParametersModel : ProductsParametersModel
    {
        public SearchProductsParametersModel()
        {
            Term = string.Empty;
            OrderBy = (int) ProductSortingEnum.CreatedOn;
        }

        /// <summary>
        /// Keyword 
        /// </summary>
        [JsonProperty("term")]
        public string Term { get; set; }

        /// <summary>
        /// Order by 
        /// </summary>
        [JsonProperty("order_by")]
        public int OrderBy { get; set; }
    }
}
