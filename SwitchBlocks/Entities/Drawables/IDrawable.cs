namespace SwitchBlocks.Entities.Drawables
{
    using JumpKing;
    using Microsoft.Xna.Framework.Graphics;

    public interface IDrawable
    {
        /// <summary>
        /// Draws a drawable.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch</param>
        /// <param name="state">The state the drawable has to take into account</param>
        /// <param name="progress">The progress the drawable has to take into account</param>
        void Draw(SpriteBatch spriteBatch, bool state, float progress);

        /// <summary>
        /// Initializes the texture/s from the given path.
        /// </summary>
        /// <param name="contentManager">The JKContentManager that provides a way to load textures</param>
        /// <param name="path">The path to the texture(s)</param>
        /// <returns>True if all textures could be initialized, false otherwise</returns>
        bool InitializeTextures(JKContentManager contentManager, string path);

        /// <summary>
        /// Initializes other fields that might be relevant.
        /// </summary>
        /// <returns>True if others could be initialized, false otherwise</returns>
        bool InitializeOthers();
    }
}
