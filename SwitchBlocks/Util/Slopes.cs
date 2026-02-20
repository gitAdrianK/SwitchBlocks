namespace SwitchBlocks.Util
{
    using JumpKing.Level;
    using JumpKing.Level.Sampler;

    /// <summary>
    ///     Similar in function to the vanilla GetSlopeType, but it is <c>private</c>
    ///     and this is easier than somehow getting it with Harmony.
    /// </summary>
    public static class Slopes
    {
        /// <summary>
        ///     Determines a <see cref="SlopeType" /> based on the surrounding blocks.
        /// </summary>
        /// <param name="textureSrc">The <see cref="LevelTexture" /> source for this block in the map file.</param>
        /// <param name="screen">The index of the current screen being parsed.</param>
        /// <param name="x">The x index of the block on the screen.</param>
        /// <param name="y">The y index of the block on the screen.</param>
        /// <returns><see cref="SlopeType" /> based on surrounding blocks.</returns>
        public static SlopeType GetSlopeType(LevelTexture textureSrc, int screen, int x, int y)
        {
            if (LevelManager.IsBlockEdge(textureSrc, screen, x + 1, y) &&
                LevelManager.IsBlockEdge(textureSrc, screen, x, y + 1))
            {
                return SlopeType.TopLeft;
            }

            if (LevelManager.IsBlockEdge(textureSrc, screen, x - 1, y) &&
                LevelManager.IsBlockEdge(textureSrc, screen, x, y + 1))
            {
                return SlopeType.TopRight;
            }

            if (LevelManager.IsBlockEdge(textureSrc, screen, x + 1, y) &&
                LevelManager.IsBlockEdge(textureSrc, screen, x, y - 1))
            {
                return SlopeType.BottomLeft;
            }

            if (LevelManager.IsBlockEdge(textureSrc, screen, x - 1, y) &&
                LevelManager.IsBlockEdge(textureSrc, screen, x, y - 1))
            {
                return SlopeType.BottomRight;
            }

            return SlopeType.None;
        }
    }
}
