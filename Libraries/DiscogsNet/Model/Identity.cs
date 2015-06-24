using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscogsNet.Model
{
    public class Identity
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string resource_url { get; set; }
        public string consumer_name { get; set; }
    }
}
