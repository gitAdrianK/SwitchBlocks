// ReSharper disable ArrangeTypeMemberModifiers

namespace SwitchBlocks.Grouping
{
    /// <summary>
    ///     Interface indicating that the block can be grouped with other blocks.
    /// </summary>
    /// <typeparam name="T">What the grouping is about.</typeparam>
    public interface IGroupable<T>
    {
        /// <summary>
        ///     The default value assigned should it not be assigned by seed.
        /// </summary>
        T DefaultValue { get; }

        /// <summary>
        ///     The current value of the grouping.
        /// </summary>
        T Value { get; set; }

        /// <summary>
        ///     If the value has been set.
        /// </summary>
        /// <returns></returns>
        bool IsSet { get; }
    }

    /// <summary>
    ///     Marker interface for blocks groupable by an id.
    /// </summary>
    public interface IGroupId : IGroupable<int>
    {
    }

    /// <summary>
    ///     Marker interface for blocks groupable by multiple ids.
    /// </summary>
    public interface IGroupIds : IGroupable<int[]>
    {
    }

    /// <summary>
    ///     Marker interface for blocks groupable by a duration.
    /// </summary>
    public interface IDuration : IGroupable<int>
    {
    }
}
