using HomeDashboard.Library.Models.Hue;
using System;
using System.Collections.Generic;

namespace HomeDashboard.Models
{
    public class HueResponse
    {
        public HueResponse()
        {
            this.Hues = new List<HueSceneDto>(); 
        }

        public DateTime LastUpdated { get; set; }
        public List<HueSceneDto> Hues { get; set; }
    }
}
