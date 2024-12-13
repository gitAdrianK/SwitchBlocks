using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Patching;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    public class EntityLevers : Entity
    {
        int currentScreen = -1;
        int nextScreen;

        protected bool state;

        public Dictionary<int, List<Lever>> LeverDictionary { get; protected set; }
        List<Lever> currentLeverList;

        protected bool UpdateCurrentScreen()
        {
            if (LeverDictionary == null)
            {
                return false;
            }

            nextScreen = Camera.CurrentScreen;
            if (currentScreen != nextScreen)
            {
                LeverDictionary.TryGetValue(nextScreen, out currentLeverList);
                currentScreen = nextScreen;
            }
            return currentLeverList != null;
        }

        public override void Draw()
        {
            if (currentLeverList == null || EndingManager.HasFinished)
            {
                return;
            }

            SpriteBatch spriteBatch = Game1.spriteBatch;
            Parallel.ForEach(currentLeverList, lever =>
            {
                DrawLever(lever, spriteBatch);
            });
        }

        private void DrawLever(Lever lever, SpriteBatch spriteBatch)
        {
            Rectangle rectangle;
            if (state)
            {
                rectangle = new Rectangle(
                    0,
                    0,
                    lever.Width,
                    lever.Height);
            }
            else
            {
                rectangle = new Rectangle(
                    lever.Width,
                    0,
                    lever.Width,
                    lever.Height);
            }
            spriteBatch.Draw(
                texture: lever.Texture,
                position: lever.Position,
                sourceRectangle: rectangle,
                color: Color.White);
        }
    }
}
