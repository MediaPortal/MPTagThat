using System.Linq;

namespace DiscogsNet.Model
{
    public class ReleaseFormat
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Text { get; set; }
        public string[] Descriptions { get; set; }

        public override string ToString()
        {
            return Name +
                (Quantity == 1 ? "" : " x" + Quantity) +
                (Descriptions == null ? "" : Descriptions.Select(d => ", " + d).Join(""))
                ;
        }
    }
}
