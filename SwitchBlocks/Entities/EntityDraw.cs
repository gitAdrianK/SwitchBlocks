namespace SwitchBlocks.Entities
{
    using EntityComponent;
    using JumpKing;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Patches;

    /// <summary>
    ///     Abstract class other drawing entities inherit from.
    /// </summary>
    public abstract class EntityDraw : Entity
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D" />.</param>
        /// <param name="position">Position.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="isForeground">Should this entity remain in front of the player.</param>
        protected EntityDraw(Texture2D texture, Vector2 position, int screen, bool isForeground)
        {
            this.Texture = texture;
            this.Height = texture.Height;
            this.Width = texture.Width;
            this.Position = position;
            this.Screen = screen;
            this.IsForeground = isForeground;
        }

        /// <summary><see cref="Texture2D" />.</summary>
        protected Texture2D Texture { get; }

        /// <summary>Position.</summary>
        protected Vector2 Position { get; }

        /// <summary>Height.</summary>
        protected int Height { get; set; }

        /// <summary>Width.</summary>
        protected int Width { get; set; }

        /// <summary>Screen this entity is on.</summary>
        protected int Screen { get; }

        /// <summary>Does the entity remain in front of the player.</summary>
        public bool IsForeground { get; }

        /// <summary>
        ///     If the draw call should be cancelled because of various reasons that prevent the entity from drawing.
        /// </summary>
        /// <returns><c>true</c> if the draw should be cancelled, <c>false</c> otherwise.</returns>
        protected bool DrawGuard() => Camera.CurrentScreen != this.Screen || PatchEndingManager.HasFinished ||
                                      LevelScreen.DrawDebug;
    }
}
