namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;

    public class EntityLogicSequence : EntityGroupLogic<DataSequence>
    {
        private int Duration { get; set; }

        public EntityLogicSequence() : base(DataSequence.Instance, SettingsSequence.Multiplier)
            => this.Duration = SettingsGroup.Duration;

        protected override void Update(float deltaTime)
        {
            var tick = AchievementManager.GetTicks();
            var multiplier = this.Multiplier;
            var finished = new List<int>();
            _ = Parallel.ForEach(this.Data.Active, group =>
            {
                var blockGroup = this.Data.Groups[group];
                this.UpdateProgress(blockGroup, deltaTime);
                this.TrySwitch(blockGroup, tick);
                if (blockGroup.Progress == Convert.ToInt32(blockGroup.State))
                {
                    // if the group is "finished", but the switch is planned in the near future,
                    // dont add it to finished just yet.
                    if (!(tick <= blockGroup.ActivatedTick && tick + this.Duration >= blockGroup.ActivatedTick))
                    {
                        lock (finished)
                        {
                            finished.Add(group);
                        }
                    }
                }
            });
            foreach (var i in finished)
            {
                var blockGroup = this.Data.Groups[i];
                if (blockGroup.State && blockGroup.Progress == 1.0f)
                {
                    _ = this.Data.Finished.Add(i);
                }
                _ = this.Data.Active.Remove(i);
            }
        }

        private void TrySwitch(BlockGroup group, int tick)
        {
            // A platform is solid if the activated tick is larger than the current tick.
            var newState = group.ActivatedTick > tick;
            if (group.State != newState)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.SequenceFlip?.PlayOneShot();
                }
                group.State = newState;
            }
        }
    }
}
