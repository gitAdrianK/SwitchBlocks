namespace SwitchBlocks.Entities
{
    using System;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util.Deserialization;

    public class EntityDrawLever : EntityDraw
    {
        private IDataProvider Data { get; }

        public EntityDrawLever(
            Lever lever,
            int screen,
            IDataProvider data)
            : base(lever.Texture, lever.Position, screen)
        {
            this.Width /= 2;
            this.Data = data;
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
                    this.Width * Convert.ToInt32(!this.Data.State),
                    0,
                    this.Width,
                    this.Height),
                color: Color.White);
        }
    }
}
