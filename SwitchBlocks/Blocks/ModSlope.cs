namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A slope block that additionally provides a quick way to check if it is solid.
    /// </summary>
    public abstract class ModSlope : SlopeBlock, IBlockDebugColor
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="collider"><see cref="Rectangle" /> to be used for this block's collider.</param>
        /// <param name="slopeType">Which of the 4 <see cref="SlopeType" />s this should be.</param>
        public ModSlope(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <summary>
        ///     If this block is solid.
        /// </summary>
        public abstract bool CanBlockPlayer { get; }

        /// <summary>
        ///     <see cref="Color" /> this block is drawn as when in debug.
        /// </summary>
        public abstract Color DebugColor { get; }
    }
}
