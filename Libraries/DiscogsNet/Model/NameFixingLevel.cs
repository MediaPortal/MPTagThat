using System;

namespace DiscogsNet.Model
{
    [Flags]
    public enum NameFixingLevel
    {
        None = 0,
        FixThe,
        RemoveNumbers,
        All = FixThe | RemoveNumbers
    }
}
