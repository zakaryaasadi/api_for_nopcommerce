using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Plugin.Api.DTOs.NivoSlider
{
    [JsonObject(Title = "nivo_slider")]
    public class NivoSliderDto
    {
        /// <summary>
        /// Gets or sets the picture url 
        /// </summary>
        [JsonProperty("picture_url")]
        public string PictureUrl { get; set; }
        /// <summary>
        /// Gets or sets a comment for this slide
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value link
        /// </summary>
        [JsonProperty("link")]

        public string Link { get; set; }

    }
}
