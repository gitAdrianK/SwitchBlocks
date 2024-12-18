using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Patching;
using System;
using System.Collections.Generic;

namespace SwitchBlocks.Entities
{
    public abstract class EntityDrawables<T> : Entity where T : IDrawable
    {
        int currentScreen = -1;

        private readonly Dictionary<int, T[]> drawablesDict;
        protected T[] currentDrawables;
        public bool IsActiveOnCurrentScreen => currentDrawables != null;

        protected abstract void EntityUpdate(float p_delta);
        public abstract void EntityDraw(SpriteBatch spriteBatch);

        protected EntityDrawables(string subfolder, string blocktype)
        {
            // TODO: Make dictionary
            drawablesDict = null;
        }

        protected override void Update(float p_delta)
        {
            EntityUpdate(p_delta);
        }

        public override void Draw()
        {
            if (UpdateCurrentScreen() && !EndingManager.HasFinished)
            {
                SpriteBatch spriteBatch = Game1.spriteBatch;
                EntityDraw(spriteBatch);
            }
        }

        /// <summary>
        /// Updates what screen is currently active and gets the platforms from the platform dictionary
        /// </summary>
        /// <returns>If no platforms are to be drawn false, true otherwise</returns>
        private bool UpdateCurrentScreen()
        {
            int nextScreen = Camera.CurrentScreen;
            if (currentScreen != nextScreen)
            {
                drawablesDict?.TryGetValue(nextScreen, out currentDrawables);
                currentScreen = nextScreen;
            }
            return currentDrawables != null;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating. Progress is clamped from zero to one.
        /// The amount is multiplied by two to keep parity with a previous bug.
        /// </summary>
        /// <param name="state">State of the platforms type</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        /// <returns>The new progress after updating</returns>
        protected float UpdateProgressClamped(bool state, float progress, float amount, float multiplier)
        {
            int stateInt = Convert.ToInt32(state);
            if (progress == stateInt)
            {
                return progress;
            }
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= (-1 + (stateInt * 2)) * 2 * multiplier;
            progress += amount;
            return Math.Min(Math.Max(progress, 0), 1);
        }
    }
}
