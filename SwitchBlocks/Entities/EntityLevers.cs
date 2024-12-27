namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EntityComponent;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util;

    public class EntityLevers : Entity
    {
        private int currentScreen = -1;
        private int nextScreen;

        protected bool State { get; set; }

        public Dictionary<int, List<Lever>> LeverDictionary { get; protected set; }
        private List<Lever> currentLeverList;

        protected bool UpdateCurrentScreen()
        {
            if (this.LeverDictionary == null)
            {
                return false;
            }

            this.nextScreen = Camera.CurrentScreen;
            if (this.currentScreen != this.nextScreen)
            {
                _ = this.LeverDictionary.TryGetValue(this.nextScreen, out this.currentLeverList);
                this.currentScreen = this.nextScreen;
            }
            return this.currentLeverList != null;
        }

        public override void Draw()
        {
            if (this.currentLeverList == null || EndingManager.HasFinished)
            {
                return;
            }

            var spriteBatch = Game1.spriteBatch;
            _ = Parallel.ForEach(this.currentLeverList, lever
                => this.DrawLever(lever, spriteBatch));
        }

        private void DrawLever(Lever lever, SpriteBatch spriteBatch)
        {
            var rectangle = new Rectangle(
                    lever.Width * Convert.ToInt32(!this.State),
                    0,
                    lever.Width,
                    lever.Height);
            spriteBatch.Draw(
                texture: lever.Texture,
                position: lever.Position,
                sourceRectangle: rectangle,
                color: Color.White);
        }
    }
}
