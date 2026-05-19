// ReSharper disable CompareOfFloatsByEqualityOperator

namespace SwitchBlocks.Entities
{
    using System.Collections.Generic;
    using Data;
    using EntityComponent;
    using JumpKing;

    /// <summary>
    ///     Abstract class other group logic entities inherit from.
    /// </summary>
    /// <typeparam name="T">Class implementing <see cref="IGroupDataProvider" />.</typeparam>
    public abstract class EntityGroupLogic<T> : Entity where T : IGroupDataProvider
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="data">Class implementing <see cref="IGroupDataProvider" />.</param>
        protected EntityGroupLogic(T data)
        {
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Finished = data.Finished;
            this.Screens = new HashSet<int>();
        }
        // Okay but I get it, technically if I want no code duplication at all
        // I need another class that both logic classes inherit from

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        protected Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active.</summary>
        protected HashSet<int> Active { get; }

        /// <summary>Cached IDs considered finished.</summary>
        protected HashSet<int> Finished { get; }

        /// <summary>Multiplier.</summary>
        protected float Multiplier { get; set; }

        /// <summary>Screens platform entities appear on.</summary>
        private HashSet<int> Screens { get; }

        /// <summary>If the current screen contains platform entities.</summary>
        protected bool IsActiveOnCurrentScreen => this.Screens.Contains(Camera.CurrentScreen);

        /// <summary>
        ///     Adds a screen as a screen a platform entity appears on.
        /// </summary>
        /// <param name="screen">Screen a platform entity appears on.</param>
        public void AddScreen(int screen) => this.Screens.Add(screen);

        /// <summary>
        ///     Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="group">Block-group with progress and state</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        protected void UpdateProgress(BlockGroup group, float amount)
        {
            if (group.Progress == (group.State ? 1.0f : 0.0f))
            {
                return;
            }

            group.Progress += amount * (group.State ? 4.0f : -4.0f) * this.Multiplier;
            if (group.Progress < 0.0f)
            {
                group.Progress = 0.0f;
            }
            else if (group.Progress > 1.0f)
            {
                group.Progress = 1.0f;
            }
        }
    }
}
