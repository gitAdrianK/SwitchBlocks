// ReSharper disable CompareOfFloatsByEqualityOperator

namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Concurrent;
    using Data;
    using Patches;
    using Settings;

    /// <summary>
    ///     Sequence logic entity.
    /// </summary>
    public class EntityLogicSequence : EntityGroupLogic<DataSequence>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public EntityLogicSequence() : base(DataSequence.Instance, SettingsSequence.Multiplier)
            => this.Duration = SettingsGroup.Duration;

        /// <summary>Duration the state lasts for.</summary>
        private int Duration { get; }

        /// <summary>
        ///     Updates progress and state of groups that are marked as active.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
        {
            var tick = PatchAchievementManager.GetTick();
            var finishedIds = new ConcurrentBag<int>();
            foreach (var groupId in this.Active)
            {
                if (!this.Groups.TryGetValue(groupId, out var group))
                {
                    continue;
                }

                this.UpdateProgress(group, deltaTime);
                this.TrySwitch(group, tick);
                if (group.Progress != Convert.ToInt32(group.State))
                {
                    continue;
                }

                // if the group is "finished", but the switch is planned in the near future,
                // don't add it to finished just yet.
                if (tick <= group.ActivatedTick && tick + this.Duration >= group.ActivatedTick)
                {
                    continue;
                }

                finishedIds.Add(groupId);
                if (group.State && group.Progress == 1.0f)
                {
                    _ = this.Finished.Add(groupId);
                }
            }

            foreach (var groupId in finishedIds)
            {
                _ = this.Active.Remove(groupId);
            }
        }

        /// <summary>
        ///     Tries to switch the state if it should do so.
        /// </summary>
        /// <param name="group">Group that is trying to switch state.</param>
        /// <param name="tick">Current game-tick.</param>
        private void TrySwitch(BlockGroup group, int tick)
        {
            // A platform is solid if the activated tick is larger than the current tick.
            var newState = group.ActivatedTick > tick;
            if (group.State == newState)
            {
                return;
            }

            if (this.IsActiveOnCurrentScreen)
            {
                ModSounds.SequenceFlip?.PlayOneShot();
            }

            group.State = newState;
        }
    }
}
