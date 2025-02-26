namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using EntityComponent;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    public abstract class EntityGroupLogic<T> : Entity where T : IGroupDataProvider
    {
        // Okay but I get it, technically if I want no code duplication at all
        // I need another class that both logic classes inherit from

        protected T Data { get; set; }
        protected float Multiplier { get; set; }

        private HashSet<int> Screens { get; set; }
        public bool IsActiveOnCurrentScreen => this.Screens.Contains(Camera.CurrentScreen);
        public void AddScreen(int screen) => this.Screens.Add(screen);

        protected EntityGroupLogic(T data, float multiplier)
        {
            this.Data = data;
            this.Multiplier = multiplier;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="group">Blockgroup with progress and state</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        protected void UpdateProgress(BlockGroup group, float amount)
        {
            var stateInt = Convert.ToInt32(group.State);
            if (group.Progress == stateInt)
            {
                return;
            }
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= (-1 + (stateInt * 2)) * 2 * this.Multiplier;
            group.Progress += amount;
            group.Progress = Math.Min(Math.Max(group.Progress, 0), 1);
        }
    }
}
