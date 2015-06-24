using System;
namespace DiscogsNet.Model
{
    public class TrackAggregate
    {
        private Track track;

        private TimeSpan? duration;
        public TimeSpan Duration
        {
            get
            {
                if (this.duration.HasValue)
                {
                    return this.duration.Value;
                }

                string value = this.track.Duration;
                if (value == "" || value == "?")
                {
                    this.duration = TimeSpan.Zero;
                    return this.duration.Value;
                }
                else
                {
                    TimeSpan parsed;
                    TimeSpanParser.TryParse(value, out parsed);
                    this.duration = parsed;
                    return this.duration.Value;
                }
            }
        }

        public string JoinedArtists
        {
            get
            {
                if (this.track.Artists == null)
                {
                    return null;
                }

                return this.track.Artists.Join();
            }
        }

        public string JoinedArtistsFixed
        {
            get
            {
                if (this.track.Artists == null)
                {
                    return null;
                }

                return this.track.Artists.JoinFixed();
            }
        }

        public TrackAggregate(Track track)
        {
            this.track = track;
        }

        public string GetJoinedArtists(Release release)
        {
            return (this.track.Artists ?? release.Artists).Join();
        }

        public string GetJoinedArtistsFixed(Release release)
        {
            return (this.track.Artists ?? release.Artists).JoinFixed();
        }
    }
}
