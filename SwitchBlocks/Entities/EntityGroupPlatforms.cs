namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities.Drawables;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Setups;
    using SwitchBlocks.Util;

    /// <summary>
    /// Entity responsible for rendering group platforms in the level.
    /// </summary>
    public class EntityGroupPlatforms : EntityDrawables<PlatformInOutGroup>
    {
        public EntityGroupPlatforms() : base(ModConsts.XML_PLATFORMS, ModConsts.GROUP)
        {
            // XXX: This is mega scuffed. I really want to link during the creation of the platform
            foreach (var kv in this.DrawablesDict)
            {
                foreach (var drawable in kv.Value)
                {
                    if (SetupGroup.BlocksGroupA.TryGetValue(drawable.FormatLink, out var value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupGroup.BlocksGroupB.TryGetValue(drawable.FormatLink, out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupGroup.BlocksGroupC.TryGetValue(drawable.FormatLink, out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupGroup.BlocksGroupD.TryGetValue(drawable.FormatLink, out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupGroup.BlocksGroupA.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupGroup.BlocksGroupB.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupGroup.BlocksGroupC.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupGroup.BlocksGroupD.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                }
            }
        }

        protected override void EntityUpdate(float p_delta)
        {
            var tick = AchievementManager.GetTicks();
            var multiplier = SettingsGroup.Multiplier;
            var finished = new List<int>();
            _ = Parallel.ForEach(DataGroup.Active, group =>
            {
                var blockGroup = DataGroup.Groups[group];
                blockGroup.Progress = this.UpdateProgressClamped(
                    blockGroup.State,
                    blockGroup.Progress,
                    p_delta,
                    multiplier);
                this.TrySwitch(blockGroup, tick);
                if (blockGroup.Progress == Convert.ToInt32(blockGroup.State))
                {
                    // if the group is "finished", but the switch is planned in the near future,
                    // dont add it to finished just yet.
                    if (!(tick <= blockGroup.ActivatedTick && tick + SettingsGroup.Duration >= blockGroup.ActivatedTick))
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
                var blockGroup = DataGroup.Groups[i];
                if (!blockGroup.State && blockGroup.Progress == 0.0f)
                {
                    DataGroup.Finished.Add(i);
                }
                _ = DataGroup.Active.Remove(i);
            }
        }

        public override void EntityDraw(SpriteBatch spriteBatch) => Parallel.ForEach(this.CurrentDrawables, drawable
            => drawable.Draw(
                spriteBatch,
                DataGroup.GetState(drawable.GroupId),
                DataGroup.GetProgress(drawable.GroupId)));

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
