using DiscogsNet.Model;
using DiscogsNet.Model.Obsolete;

namespace DiscogsNet
{
    public interface IDiscogsApi
    {
        Release GetRelease(int id);
        Artist GetArtist(string artistName);
        Label GetLabel(string labelName);
        SearchResults_Obsolete Search(string searchString);
    }
}
