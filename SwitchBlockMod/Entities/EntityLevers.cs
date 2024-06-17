using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SwitchBlocksMod.Entities
{
    public class EntityLevers : Entity
    {

        protected bool state;

        public Dictionary<int, List<Lever>> LeverDictionary { get; protected set; }
        List<Lever> currentLeverList;

        protected bool UpdateCurrentScreen()
        {
            if (LeverDictionary == null)
            {
                return false;
            }
            int currentScreen = Camera.CurrentScreen;
            if (LeverDictionary.ContainsKey(currentScreen))
            {
                currentLeverList = LeverDictionary[currentScreen];
                return true;
            }
            else
            {
                currentLeverList = null;
                return false;
            }
        }

        public override void Draw()
        {
            if (currentLeverList == null)
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
                    lever.texture.Width / 2,
                    lever.texture.Height);
            }
            else
            {
                rectangle = new Rectangle(
                    lever.texture.Width / 2,
                    0,
                    lever.texture.Width / 2,
                    lever.texture.Height);
            }
            spriteBatch.Draw(
                texture: lever.texture,
                position: lever.position,
                sourceRectangle: rectangle,
                color: Color.White);
        }
    }
}
