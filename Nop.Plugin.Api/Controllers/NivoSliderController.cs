using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Api.Attributes;
using Nop.Plugin.Api.DTO.Errors;
using Nop.Plugin.Api.JSON.Serializers;
using Nop.Plugin.Widgets.NivoSlider;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Plugin.Api.DTOs.NivoSlider;
using System.Collections.Generic;
using System.Net;
using Nop.Plugin.Api.JSON.ActionResults;

namespace Nop.Plugin.Api.Controllers
{
    public class NivoSliderController : BaseApiController
    {
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        public NivoSliderController(
            IJsonFieldsSerializer jsonFieldsSerializer,
            IAclService aclService,
            ICustomerService customerService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IDiscountService discountService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IStoreContext storeContext,
            ISettingService settingService) : base(jsonFieldsSerializer, aclService, customerService, storeMappingService, storeService, discountService, customerActivityService, localizationService, pictureService)
        {
            _storeContext = storeContext;
            _settingService = settingService;
            _pictureService = pictureService;
        }


        /// <summary>
        /// Receive a list of all Sliders
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/sliders")]
        [ProducesResponseType(typeof(NivoSlidersRootObject), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetNivoSliders()
        {

            var nivoSliderSettings = _settingService.LoadSetting<NivoSliderSettings>(_storeContext.CurrentStore.Id);

            var slidesAsDtos = GetNivoSlider(nivoSliderSettings);

            var nivoSlidersRootObject = new NivoSlidersRootObject()
            {
                Slider = slidesAsDtos
            };

            var json = JsonFieldsSerializer.Serialize(nivoSlidersRootObject, "");

            return new RawJsonActionResult(json);
        }


        private IList<NivoSliderDto> GetNivoSlider(NivoSliderSettings nivoSliderSettings)
        {
            var sliders = new List<NivoSliderDto>();

            var Picture1Url = _pictureService.GetPictureUrl(nivoSliderSettings.Picture1Id, showDefaultPicture: false) ?? "";

            var Picture2Url = _pictureService.GetPictureUrl(nivoSliderSettings.Picture2Id, showDefaultPicture: false) ?? "";

            var Picture3Url = _pictureService.GetPictureUrl(nivoSliderSettings.Picture3Id, showDefaultPicture: false) ?? "";

            var Picture4Url = _pictureService.GetPictureUrl(nivoSliderSettings.Picture4Id, showDefaultPicture: false) ?? "";

            var Picture5Url = _pictureService.GetPictureUrl(nivoSliderSettings.Picture5Id, showDefaultPicture: false) ?? "";

            if (!string.IsNullOrEmpty(Picture1Url))
            {
                sliders.Add(new NivoSliderDto()
                {
                    PictureUrl = Picture1Url,
                    Text = nivoSliderSettings.Text1,
                    Link = nivoSliderSettings.Link1
                });
            }

            if (!string.IsNullOrEmpty(Picture2Url))
            {
                sliders.Add(new NivoSliderDto()
                {
                    PictureUrl = Picture2Url,
                    Text = nivoSliderSettings.Text2,
                    Link = nivoSliderSettings.Link2
                });
            }

            if (!string.IsNullOrEmpty(Picture3Url))
            {
                sliders.Add(new NivoSliderDto()
                {
                    PictureUrl = Picture3Url,
                    Text = nivoSliderSettings.Text3,
                    Link = nivoSliderSettings.Link3
                });
            }


            if (!string.IsNullOrEmpty(Picture4Url))
            {
                sliders.Add(new NivoSliderDto()
                {
                    PictureUrl = Picture4Url,
                    Text = nivoSliderSettings.Text4,
                    Link = nivoSliderSettings.Link4
                });
            }


            if (!string.IsNullOrEmpty(Picture5Url))
            {
                sliders.Add(new NivoSliderDto()
                {
                    PictureUrl = Picture5Url,
                    Text = nivoSliderSettings.Text5,
                    Link = nivoSliderSettings.Link5
                });
            }



            return sliders;
        }
    }
}
