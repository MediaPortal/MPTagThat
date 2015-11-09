using System;

namespace DiscogsNet
{
    public interface IAsyncDiscogsReader<T> : IDisposable
    {
        bool IsConcurrent { get; set; }
        void ReadAll(Func<int, T, bool> processor);
        double EstimatedProgress { get; }
    }
}
