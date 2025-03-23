namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public abstract class ModBlock : IBlock, IBlockDebugColor
    {
        /// <summary>
        /// This blocks <see cref="Rectangle"/> used for collision.
        /// </summary>
        protected Rectangle Collider { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="collider">Collider to be used for this block.</param>
        protected ModBlock(Rectangle collider) => this.Collider = collider;

        /// <summary>
        /// <see cref="Color"/> this block is drawn as when in debug.
        /// </summary>
        public abstract Color DebugColor { get; }

        /// <summary>
        /// <see cref="Rectangle"/> this block is drawn as when in debug.
        /// </summary>
        /// <returns></returns>
        public abstract Rectangle GetRect();

        /// <summary>
        /// Type of collision this collider intersects with another (the players) <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="hitbox"><see cref="Rectangle"/> to check against.</param>
        /// <param name="intersection">Overlap of the two <see cref="Rectangle"/>s.</param>
        /// <returns><see cref="BlockCollisionType"/> depending on the type of collision.</returns>
        public abstract BlockCollisionType Intersects(Rectangle p_hitbox, out Rectangle p_intersection);
    }
}
