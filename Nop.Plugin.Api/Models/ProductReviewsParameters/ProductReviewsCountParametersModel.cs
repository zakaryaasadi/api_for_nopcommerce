using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Api.ModelBinders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Api.Models.ProductReviewsParameters
{
    [ModelBinder(typeof(ParametersModelBinder<ProductReviewsCountParametersModel>))]
    public class ProductReviewsCountParametersModel : BaseProductReviewsParametersModel
    {
        public ProductReviewsCountParametersModel()
        {
            // Nothing special here, created just for clarity.
        }
    }
}
