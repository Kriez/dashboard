namespace kriez.HomeDashboard.Data.Models
{
    public class HueLight
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? IsReachable { get; set; }
        public bool IsOn { get; set; }
        public string Group { get; set; }
        public int Brightness { get; set; }
        public string Color { get; set; }

        public bool Equals(HueLight obj)
        {
            if (!Id.Equals(obj.Id))
            {
                return false;
            }
            if (!Name.Equals(obj.Name))
            {
                return false;
            }
            if (!IsReachable.Equals(obj.IsReachable))
            {
                return false;
            }
            if (!IsOn.Equals(obj.IsOn))
            {
                return false;
            }
            if (!IsOn.Equals(obj.IsOn))
            {
                return false;
            }
            if (!Brightness.Equals(obj.Brightness))
            {
                return false;
            }
            if (!Color.Equals(obj.Color))
            {
                return false;
            }
            return true;
        }
    }
}
