using System.Collections.Generic;
using System.Linq;

namespace kriez.HomeDashboard.Data.Models
{
    public class HueScene
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<HueLight> Lights { get; set; }

        public bool Equals(HueScene obj)
        {
            if (!obj.Id.Equals(Id))
            {
                return false;
            }
            if (!obj.Name.Equals(Name))
            {
                return false;
            }

            if (obj.Lights == null && this.Lights != null)
            {
                return false;
            }
            if (obj.Lights != null && this.Lights == null)
            {
                return false;
            }

            if (obj.Lights == null && this.Lights == null)
            {
                return true;
            }

            if (!obj.Lights.Count.Equals(Lights.Count))
            {
                return false;
            }

            foreach (var item in obj.Lights)
            {
                if (!Lights.Any(l => l.Id.Equals(item.Id)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
