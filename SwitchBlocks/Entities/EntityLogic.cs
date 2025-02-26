namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using EntityComponent;
    using JumpKing;
    using SwitchBlocks.Data;

    public abstract class EntityLogic<T> : Entity where T : IDataProvider
    {
        protected T Data { get; set; }
        protected float Multiplier { get; set; }

        private HashSet<int> Screens { get; set; }
        public bool IsActiveOnCurrentScreen => this.Screens.Contains(Camera.CurrentScreen);
        public void AddScreen(int screen) => this.Screens.Add(screen);

        protected EntityLogic(T data, float multiplier)
        {
            this.Data = data;
            this.Multiplier = multiplier;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="state">State of the platforms type</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        protected void UpdateProgress(bool state, float amount)
        {
            var stateInt = Convert.ToInt32(state);
            if (this.Data.Progress == stateInt)
            {
                return;
            }
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= (-1 + (stateInt * 2)) * 2 * this.Multiplier;
            this.Data.Progress += amount;
            this.Data.Progress = Math.Min(Math.Max(this.Data.Progress, 0), 1);
        }
    }
}
