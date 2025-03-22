namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using EntityComponent;
    using JumpKing;
    using SwitchBlocks.Data;

    /// <summary>
    /// Abstract class other logic entities inherit from.
    /// </summary>
    /// <typeparam name="T">Class implementing<see cref="IDataProvider"/>.</typeparam>
    public abstract class EntityLogic<T> : Entity where T : IDataProvider
    {
        /// <summary>Class implementing <see cref="IDataProvider"/>.</summary>
        protected T Data { get; set; }
        /// <summary>Multiplier.</summary>
        protected float Multiplier { get; set; }
        /// <summary>Screens platform entites appear on.</summary>
        private HashSet<int> Screens { get; set; }
        /// <summary>If the current screen contains platform entites.</summary>
        public bool IsActiveOnCurrentScreen => this.Screens.Contains(Camera.CurrentScreen);
        /// <summary>
        /// Adds a screen as a screen a platform entity appears on.
        /// </summary>
        /// <param name="screen">Screen a platform entity appears on.</param>
        public void AddScreen(int screen) => this.Screens.Add(screen);

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="data">Class implementing <see cref="IDataProvider"/>.</param>
        /// <param name="multiplier">Multiplier.</param>
        protected EntityLogic(T data, float multiplier)
        {
            this.Data = data;
            this.Multiplier = multiplier;
            this.Screens = new HashSet<int>();
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
