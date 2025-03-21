namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Util;

    public class EntityLogicGroup : EntityGroupLogic<DataGroup>
    {
        private int Duration { get; set; }

        public EntityLogicGroup() : base(DataGroup.Instance, SettingsGroup.Multiplier)
            => this.Duration = SettingsGroup.Duration;

        protected override void Update(float deltaTime)
        {
            var tick = AchievementManager.GetTicks();
            var multiplier = this.Multiplier;
            var finished = new List<int>();
            _ = Parallel.ForEach(this.Data.Active, group =>
            {
                if (!this.Data.Groups.TryGetValue(group, out var blockGroup))
                {
                    return;
                }
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
                if (!this.Data.Groups.TryGetValue(i, out var blockGroup1))
                {
                    continue;
                }
                if (!blockGroup1.State && blockGroup1.Progress == 0.0f)
                {
                    this.Data.Finished.Add(i);
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
                    ModSounds.GroupFlip?.PlayOneShot();
                }
                group.State = newState;
            }
        }
    }
}
