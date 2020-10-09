using Newtonsoft.Json;
using Nop.Plugin.Api.DTO;
using System;
using System.Collections.Generic;

namespace Nop.Plugin.Api.DTOs.NivoSlider
{
    public class NivoSlidersRootObject : ISerializableObject
    {
        public NivoSlidersRootObject()
        {
            Slider = new List<NivoSliderDto>();
        }

        [JsonProperty("sliders")]
        public IList<NivoSliderDto> Slider { get; set; }
        public string GetPrimaryPropertyName()
        {
            return "sliders";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof(NivoSliderDto);
        }
    }
}
