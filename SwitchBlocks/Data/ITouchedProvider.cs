// ReSharper disable ArrangeTypeMemberModifiers

namespace SwitchBlocks.Data
{
    using System.Collections.Generic;

    /// <summary>
    ///     Interface giving access to a ids that have been touched.
    /// </summary>
    public interface ITouchedProvider
    {
        /// <summary>The current touched id.</summary>
        HashSet<int> Touched { get; }
    }
}
