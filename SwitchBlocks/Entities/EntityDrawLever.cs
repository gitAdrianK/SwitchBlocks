namespace SwitchBlocks.Entities
{
    using System;
    using Data;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Patches;
    using Util.Deserialization;

    /// <summary>
    ///     Lever drawn based on data.
    /// </summary>
    public class EntityDrawLever : EntityDraw
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="lever">Deserialization helper <see cref="Lever" />.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        public EntityDrawLever(
            Lever lever,
            int screen,
            IDataProvider data)
            : base(lever.Texture, lever.Position, screen, lever.IsForeground)
        {
            this.Width /= 2;
            this.Data = data;
        }

        /// <summary><see cref="IDataProvider" />.</summary>
        private IDataProvider Data { get; }

        /// <summary>
        ///     Draws the entity if the current screen is the screen it appears on or the game has not finished yet.
        ///     Based on state given by the <see cref="IDataProvider" /> the left of right half of the texture is drawn.
        /// </summary>
        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || PatchEndingManager.HasFinished)
            {
                return;
            }

            Game1.spriteBatch.Draw(
                this.Texture,
                this.Position,
                new Rectangle(
                    this.Width * Convert.ToInt32(!this.Data.State),
                    0,
                    this.Width,
                    this.Height),
                Color.White);
        }
    }
}
