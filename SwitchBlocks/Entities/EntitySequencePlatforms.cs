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
    /// Entity responsible for rendering sequence platforms in the level.
    /// </summary>
    public class EntitySequencePlatforms : EntityDrawables<PlatformInOutGroup>
    {
        public EntitySequencePlatforms() : base(ModConsts.XML_PLATFORMS, ModConsts.SEQUENCE)
        {
            // XXX: This is mega scuffed. I really want to link during the creation of the platform
            foreach (var kv in this.DrawablesDict)
            {
                foreach (var drawable in kv.Value)
                {
                    if (SetupSequence.BlocksSequenceA.TryGetValue(drawable.FormatLink, out var value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupSequence.BlocksSequenceB.TryGetValue(drawable.FormatLink, out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupSequence.BlocksSequenceC.TryGetValue(drawable.FormatLink, out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupSequence.BlocksSequenceD.TryGetValue(drawable.FormatLink, out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupSequence.BlocksSequenceA.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupSequence.BlocksSequenceB.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupSequence.BlocksSequenceC.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                    if (SetupSequence.BlocksSequenceD.TryGetValue(drawable.FormatPosition(kv.Key), out value))
                    {
                        drawable.GroupId = value.GroupId;
                        continue;
                    }
                }
            }
        }

        protected override void EntityUpdate(float p_delta)
        {
            // FIXME: Platform render state wrong way around, on <-> off
            var tick = AchievementManager.GetTicks();
            var multiplier = SettingsSequence.Multiplier;
            var finished = new List<int>();
            _ = Parallel.ForEach(DataSequence.Active, group =>
            {
                var blockGroup = DataSequence.Groups[group];
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
                    if (!(tick <= blockGroup.ActivatedTick && tick + SettingsSequence.Duration >= blockGroup.ActivatedTick))
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
                var blockGroup = DataSequence.Groups[i];
                if (blockGroup.State && blockGroup.Progress == 1.0f)
                {
                    DataSequence.Finished.Add(i);
                }
                _ = DataSequence.Active.Remove(i);
            }
        }

        public override void EntityDraw(SpriteBatch spriteBatch) => Parallel.ForEach(this.CurrentDrawables, drawable
            => drawable.Draw(
                spriteBatch,
                DataSequence.GetState(drawable.GroupId),
                DataSequence.GetProgress(drawable.GroupId)));

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
