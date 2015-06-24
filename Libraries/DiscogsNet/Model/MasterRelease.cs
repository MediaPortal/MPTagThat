using System;
using System.Linq;

namespace DiscogsNet.Model
{
    public class MasterRelease : ReleaseBase
    {
        public int MainRelease { get; set; }
        public int Year { get; set; }

        [Obsolete("The new JSON API doesn't return this. Instead, users should make another request to the API.")]
        public ReleaseVersion[] Versions { get; set; }

        public MasterReleaseAggregate Aggregate { get; private set; }

        public MasterRelease()
        {
            this.Aggregate = new MasterReleaseAggregate(this);
        }

        public override string ToString()
        {
            return this.Aggregate.JoinedArtistsFixed + " - " + this.Title;
        }
    }
}
