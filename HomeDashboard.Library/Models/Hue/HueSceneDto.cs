using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDashboard.Library.Models.Hue
{
    public class HueSceneDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<HueLightDto> Lights { get; set; }
    }
}
