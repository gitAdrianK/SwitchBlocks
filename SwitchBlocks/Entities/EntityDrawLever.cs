namespace SwitchBlocks.Entities
{
    using System;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;

    public class EntityDrawLever : EntityDraw
    {
        private IDataProvider Logic { get; }

        public EntityDrawLever(
            Texture2D texture,
            Vector2 position,
            int screen,
            IDataProvider logic)
            : base(texture, position, screen)
        {
            this.Width = texture.Width / 2;
            this.Height = texture.Height;
            this.Logic = logic;
        }

        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || EndingManager.HasFinished)
            {
                return;
            }
            Game1.spriteBatch.Draw(
                texture: this.Texture,
                position: this.Position,
                sourceRectangle: new Rectangle(
                    this.Width * Convert.ToInt32(!this.Logic.State),
                    0,
                    this.Width,
                    this.Height),
                color: Color.White);
        }
    }
}
