using EntityComponent;
using JumpKing;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Data;
using System.Collections.Generic;

namespace SwitchBlocksMod.Entities
{
    /// <summary>
    /// Entity responsible for rendering sand levers in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntitySandLevers : Entity
    {
        private static EntitySandLevers instance;
        public static EntitySandLevers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntitySandLevers();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private EntitySandLevers()
        {
            LeverDictionary = Lever.GetLeversDictonary("sand");
        }

        int currentScreen = -1;
        int nextScreen;

        public Dictionary<int, List<Lever>> LeverDictionary { get; private set; }
        List<Lever> currentLeverList;

        protected override void Update(float deltaTime)
        {
            if (LeverDictionary == null)
            {
                return;
            }

            nextScreen = LevelManager.CurrentScreen.GetIndex0();
            if (currentScreen != nextScreen)
            {
                if (LeverDictionary.ContainsKey(nextScreen))
                {
                    currentLeverList = LeverDictionary[nextScreen];
                }
                else
                {
                    currentLeverList = null;
                }
                currentScreen = nextScreen;
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
            if (DataSand.State)
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
