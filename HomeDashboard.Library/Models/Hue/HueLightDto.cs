using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDashboard.Library.Models.Hue
{
    public class HueLightDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? IsReachable { get; set; }
        public bool IsOn { get; set; }
        public int Brightness { get; set; }
        public string Color { get; set; }
    }
}
