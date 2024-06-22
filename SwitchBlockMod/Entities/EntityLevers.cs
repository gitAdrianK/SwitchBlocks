using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SwitchBlocksMod.Entities
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
                currentLeverList = null;
                if (LeverDictionary.ContainsKey(nextScreen))
                {
                    currentLeverList = LeverDictionary[nextScreen];
                }
                currentScreen = nextScreen;
            }

            if (currentLeverList == null)
            {
                return false;
            }
            return true;
        }

        public override void Draw()
        {
            if (currentLeverList == null || ModEntry.HasFinished)
            {
                return;
            }

            SpriteBatch spriteBatch = Game1.spriteBatch;
            foreach (Lever lever in currentLeverList)
            {
                DrawLever(lever, spriteBatch);
            }
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
