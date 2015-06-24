using System;

namespace DiscogsNet.Api
{
    public class DiscogsApiException : Exception
    {
        public string DiscogsErrorMessage { get; private set; }

        public DiscogsApiException(string discogsErrorMessage)
            : base("The following error was returned from Discogs API call: " + discogsErrorMessage)
        {
            this.DiscogsErrorMessage = discogsErrorMessage;
        }
    }
}
