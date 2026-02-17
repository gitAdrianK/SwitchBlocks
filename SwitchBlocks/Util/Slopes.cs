namespace SwitchBlocks.Util
{
    using JumpKing.Level;
    using JumpKing.Level.Sampler;

    public static class Slopes
    {
        public static SlopeType GetSlopeType(LevelTexture src, int screen, int x, int y)
        {
            if (LevelManager.IsBlockEdge(src, screen, x + 1, y) && LevelManager.IsBlockEdge(src, screen, x, y + 1))
            {
                return SlopeType.TopLeft;
            }

            if (LevelManager.IsBlockEdge(src, screen, x - 1, y) && LevelManager.IsBlockEdge(src, screen, x, y + 1))
            {
                return SlopeType.TopRight;
            }

            if (LevelManager.IsBlockEdge(src, screen, x + 1, y) && LevelManager.IsBlockEdge(src, screen, x, y - 1))
            {
                return SlopeType.BottomLeft;
            }

            if (LevelManager.IsBlockEdge(src, screen, x - 1, y) && LevelManager.IsBlockEdge(src, screen, x, y - 1))
            {
                return SlopeType.BottomRight;
            }

            return SlopeType.None;
        }
    }
}
