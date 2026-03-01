namespace SwitchBlocks.Util
{
    /// <summary>Provides access to a speed variable. Used for conveyor blocks.</summary>
    public interface IConveyor
    {
        // ReSharper disable once ArrangeTypeMemberModifiers
        /// <summary>The speed of the conveyor.</summary>
        float Speed { get; }
    }
}
