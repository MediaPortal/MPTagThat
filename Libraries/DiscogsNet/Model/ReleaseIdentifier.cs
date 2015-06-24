using System;
using System.Linq;

namespace DiscogsNet.Model
{
    public class ReleaseIdentifier
    {
        public ReleaseIdentifierType Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
