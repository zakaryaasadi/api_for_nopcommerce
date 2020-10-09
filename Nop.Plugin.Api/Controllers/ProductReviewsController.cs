using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Api.Attributes;
using Nop.Plugin.Api.Delta;
using Nop.Plugin.Api.DTO.Errors;
using Nop.Plugin.Api.DTOs.ProductReview;
using Nop.Plugin.Api.Helpers;
using Nop.Plugin.Api.JSON.ActionResults;
using Nop.Plugin.Api.JSON.Serializers;
using Nop.Plugin.Api.ModelBinders;
using Nop.Plugin.Api.Models.ProductReviewsParameters;
using Nop.Plugin.Api.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static Nop.Plugin.Api.Infrastructure.Constants;

namespace Nop.Plugin.Api.Controllers
{
    public class ProductReviewsController : BaseApiController
    {
        #region Fields

        private readonly IProductReviewApiService _productReviewApiService;
        private readonly IDTOHelper _dtoHelper;
        private readonly IProductService _productService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly CatalogSettings _catalogSettings;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStoreContext _storeContext;
        private readonly IReviewTypeService _reviewTypeService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor


        public ProductReviewsController(IJsonFieldsSerializer jsonFieldsSerializer,
            IAclService aclService,
            ICustomerService customerService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IDiscountService discountService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IProductReviewApiService productReviewApiService,
            IDTOHelper dTOHelper,
            IProductService productService,
            IOrderService orderService,
            CatalogSettings catalogSettings,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            IEventPublisher eventPublisher,
            IStoreContext storeContext,
            IReviewTypeService reviewTypeService) : base(jsonFieldsSerializer, aclService, customerService, storeMappingService, storeService, discountService, customerActivityService, localizationService, pictureService)
        {
            _productReviewApiService = productReviewApiService;
            _dtoHelper = dTOHelper;
            _productService = productService;
            _localizationService = localizationService;
            _customerService = customerService;
            _orderService = orderService;
            _catalogSettings = catalogSettings;
            _workflowMessageService = workflowMessageService;
            _localizationSettings = localizationSettings;
            _customerActivityService = customerActivityService;
            _eventPublisher = eventPublisher;
            _storeContext = storeContext;
            _reviewTypeService = reviewTypeService;
            _storeService = storeService;
        }

        #endregion


        #region Product Review

        /// <summary>
        /// Receive a list of all Product reviews
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/productreviews")]
        [ProducesResponseType(typeof(ProductReviewsRootObject), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetProductReviews(ProductReviewsParametersModel parameters)
        {
            if (parameters.Limit < Configurations.MinLimit || parameters.Limit > Configurations.MaxLimit)
            {
                return Error(HttpStatusCode.BadRequest, "limit", "Invalid limit parameter");
            }

            if (parameters.Page < Configurations.DefaultPageValue)
            {
                return Error(HttpStatusCode.BadRequest, "page", "Invalid page parameter");
            }

            var allReviews = _productReviewApiService.GetProductReviews(parameters.CustomerId,
                parameters.ProductId,
                parameters.Ids, parameters.CreatedAtMin, parameters.CreatedAtMax,
                parameters.Limit, parameters.Page, parameters.SinceId)
                .OrderByDescending(pr => pr.Id);

            IList<ProductReviewDto> productReviewsAsDtos = allReviews.Select(pr =>
            {
                return _dtoHelper.PrepareProductReviewDTO(pr);

            }).ToList();

            var productReviewsRootObject = new ProductReviewsRootObject()
            {
                ProductReviews = productReviewsAsDtos
            };

            var json = JsonFieldsSerializer.Serialize(productReviewsRootObject, parameters.Fields);

            return new RawJsonActionResult(json);
        }

        /// <summary>
        /// Receive a count of all Product reviews
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/productreviews/count")]
        [ProducesResponseType(typeof(ProductReviewsCountRootObject), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetProductReviewsCount(ProductReviewsCountParametersModel parameters)
        {

            var reviewsCount = _productReviewApiService.GetProductReviewsCount(parameters.CustomerId,
                parameters.ProductId,
                parameters.CreatedAtMin, parameters.CreatedAtMax,
                parameters.SinceId);



            var productReviewsCountRootObject = new ProductReviewsCountRootObject()
            {
                Count = reviewsCount
            };

            return Ok(productReviewsCountRootObject);

        }


        /// <summary>
        /// Retrieve order by spcified id
        /// </summary>
        ///   /// <param name="id">Id of the order</param>
        /// <param name="fields">Fields from the order you want your json to contain</param>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/productreviews/{id}")]
        [ProducesResponseType(typeof(ProductReviewsRootObject), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetOrderById(int id, string fields = "")
        {
            if (id <= 0)
            {
                return Error(HttpStatusCode.BadRequest, "id", "invalid id");
            }

            var review = _productReviewApiService.GetProductReviewById(id);

            if (review == null)
            {
                return Error(HttpStatusCode.NotFound, "review", "not found");
            }

            var productReviewsRootObject = new ProductReviewsRootObject();

            var reviewDto = _dtoHelper.PrepareProductReviewDTO(review);
            productReviewsRootObject.ProductReviews.Add(reviewDto);

            var json = JsonFieldsSerializer.Serialize(productReviewsRootObject, fields);

            return new RawJsonActionResult(json);
        }


        [HttpPost]
        [Route("/api/productreviews")]
        [ProducesResponseType(typeof(ProductReviewsRootObject), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), 422)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public IActionResult CreateProductReview([ModelBinder(typeof(JsonModelBinder<ProductReviewDto>))]
        Delta<ProductReviewDto> productReviewDelta)
        {
            // Here we display the errors if the validation has failed at some point.
            if (!ModelState.IsValid)
            {
                return Error();
            }

            // We doesn't have to check for value because this is done by the review validator.
            var customer = CustomerService.GetCustomerById(productReviewDelta.Dto.CustomerId);

            if (customer == null)
            {
                return Error(HttpStatusCode.NotFound, "customer", "not found");
            }

            var product = _productService.GetProductById(productReviewDelta.Dto.ProductId);

            if (product == null || product.Deleted || !product.Published || !product.AllowCustomerReviews)
            {
                return Error(HttpStatusCode.NotFound, "product", "not found");
            }


            if (_customerService.IsGuest(customer) && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
            {
                return Error(HttpStatusCode.BadRequest, "customer", _localizationService.GetResource("Reviews.OnlyRegisteredUsersCanWriteReviews"));
            }

            if (_catalogSettings.ProductReviewPossibleOnlyAfterPurchasing)
            {
                var hasCompletedOrders = _orderService.SearchOrders(customerId: customer.Id,
                    productId: product.Id,
                    osIds: new List<int> { (int)OrderStatus.Complete },
                    pageSize: 1).Any();

                if (!hasCompletedOrders)
                    return Error(HttpStatusCode.BadRequest, "error", _localizationService.GetResource("Reviews.ProductReviewPossibleOnlyAfterPurchasing"));
            }

            var store = _storeService.GetStoreById(productReviewDelta.Dto.StoreId);

            //save review
            var newReview = new ProductReview()
            {
                ProductId = product.Id,
                CustomerId = customer.Id,
                StoreId = store == null ? _storeContext.CurrentStore.Id : store.Id,
                Title = productReviewDelta.Dto.Title,
                ReviewText = productReviewDelta.Dto.ReviewText,
                IsApproved = !_catalogSettings.ProductReviewsMustBeApproved,
                CreatedOnUtc = DateTime.UtcNow,
                HelpfulNoTotal = 0,
                HelpfulYesTotal = 0,
                CustomerNotifiedOfReply = _catalogSettings.NotifyCustomerAboutProductReviewReply
            };

            // must be rating betweent 1 - 5 or set default rating
            var rating = productReviewDelta.Dto.Rating;
            newReview.Rating = rating < 1 || rating > 5 ?
                _catalogSettings.DefaultProductRatingValue : rating;


            _productService.InsertProductReview(newReview);

            //add product review and review type mapping                
            foreach (var additionalReview in productReviewDelta.Dto.ReviewTypeMappingsDto)
            {

                // must be rating betweent 1 - 5 or set default rating
                var reviewTypeMappingRating = additionalReview.Rating;
                reviewTypeMappingRating = reviewTypeMappingRating < 1 || reviewTypeMappingRating > 5 ?
                    _catalogSettings.DefaultProductRatingValue : reviewTypeMappingRating;

                var reviewType = _reviewTypeService.GetReviewTypeById(additionalReview.ReviewTypeId);
                if(reviewType == null)
                {
                    // remove new Review after insert
                    _productService.DeleteProductReview(newReview);
                    return Error(HttpStatusCode.NotFound, "review_type", "not found id = " + additionalReview.ReviewTypeId);
                }

                var additionalProductReview = new ProductReviewReviewTypeMapping
                {
                    ProductReviewId = newReview.Id,
                    ReviewTypeId = reviewType.Id,
                    Rating = reviewTypeMappingRating
                };

                _reviewTypeService.InsertProductReviewReviewTypeMappings(additionalProductReview);
            }

            //update product totals
            _productService.UpdateProductReviewTotals(product);

            //notify store owner
            if (_catalogSettings.NotifyStoreOwnerAboutNewProductReviews)
                _workflowMessageService.SendProductReviewNotificationMessage(newReview, _localizationSettings.DefaultAdminLanguageId);


            //activity log
            _customerActivityService.InsertActivity(customer, "PublicStore.AddProductReview",
                string.Format(_localizationService.GetResource("ActivityLog.PublicStore.AddProductReview"), product.Name), product);

            //raise event
            if (newReview.IsApproved)
                _eventPublisher.Publish(new ProductReviewApprovedEvent(newReview));

            if (!newReview.IsApproved)
                return Ok(_localizationService.GetResource("Reviews.SeeAfterApproving"));

            var productReviewsRootObject = new ProductReviewsRootObject();
            productReviewsRootObject.ProductReviews.Add(_dtoHelper.PrepareProductReviewDTO(newReview));


            var json = JsonFieldsSerializer.Serialize(productReviewsRootObject, string.Empty);

            return new RawJsonActionResult(json);

        }


        [HttpPost]
        [Route("/api/productreviews/helpfulness")]
        [ProducesResponseType(typeof(ErrorsRootObject), 422)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public IActionResult SetProductReviewHelpfulness([ModelBinder(typeof(JsonModelBinder<ProductReviewHelpfulnessDto>))]
        Delta<ProductReviewHelpfulnessDto> helpfulnessDelta)
        {
            // Here we display the errors if the validation has failed at some point.
            if (!ModelState.IsValid)
            {
                return Error();
            }

            var dto = helpfulnessDelta.Dto;

            var productReview = _productService.GetProductReviewById(dto.ProductReviewId);
            if (productReview == null)
                return Error(HttpStatusCode.NotFound, "product_review", "No product review found with the specified id");

            var customer = _customerService.GetCustomerById(dto.CustomerId);
            if(customer == null)
                return Error(HttpStatusCode.NotFound, "customer", "No customer found with the specified id");

            if (_customerService.IsGuest(customer) && !_catalogSettings.AllowAnonymousUsersToReviewProduct)
            {
                return Error(HttpStatusCode.BadRequest, "customer", _localizationService.GetResource("Reviews.Helpfulness.OnlyRegistered"));
            }


            //customers aren't allowed to vote for their own reviews
            if (productReview.CustomerId == customer.Id)
            {
                return Error(HttpStatusCode.BadRequest, "customer", _localizationService.GetResource("Reviews.Helpfulness.YourOwnReview"));
            }


            _productService.SetProductReviewHelpfulness(productReview, dto.Washelpful);

            //new totals
            _productService.UpdateProductReviewHelpfulnessTotals(productReview);

            return Json(new
            {
                Result = _localizationService.GetResource("Reviews.Helpfulness.SuccessfullyVoted"),
                TotalYes = productReview.HelpfulYesTotal,
                TotalNo = productReview.HelpfulNoTotal
            });

        }

        #endregion
    }
}
