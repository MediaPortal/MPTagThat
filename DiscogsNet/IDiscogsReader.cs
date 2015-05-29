using System.Collections.Generic;

namespace DiscogsNet
{
    public interface IDiscogsReader<T>
    {
        T Read();
        IEnumerable<T> Enumerate();
        double EstimatedProgress { get; }
    }
}
