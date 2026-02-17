namespace SwitchBlocks.Blocks
{
    using System;
    using ErikMaths;
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
        /// <param name="collider">Collider to be used for this block.</param>
        /// <param name="slopeType">Which of the 4 slope types this should be.</param>
        public ModSlope(Rectangle collider, SlopeType slopeType) : base(collider, slopeType)
        {
            switch (slopeType)
            {
                // We don't care about what's the front, so two types have the same diagonal.
                case SlopeType.TopLeft:
                case SlopeType.BottomRight:
                    this.Diagonal = new Line
                    {
                        p0 = new Point(collider.Right, collider.Top),
                        p1 = new Point(collider.Left, collider.Bottom),
                    };
                    break;
                case SlopeType.TopRight:
                case SlopeType.BottomLeft:
                    this.Diagonal = new Line
                    {
                        p0 = new Point(collider.Left, collider.Top),
                        p1 = new Point(collider.Right, collider.Bottom),
                    };
                    break;
                case SlopeType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slopeType), slopeType, null);
            }
        }

        /// <summary>
        ///     The diagonal <see cref="Line" /> of the slope triangle.
        /// </summary>
        public Line Diagonal { get; private set; }

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
