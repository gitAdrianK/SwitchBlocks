using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Patching;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering group platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityGroupPlatforms : EntityDrawables<PlatformInOutGroup>
    {
        private static EntityGroupPlatforms instance;
        public static EntityGroupPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityGroupPlatforms();
                }
                return instance;
            }
        }

        public override void Reset()
        {
            instance.Destroy();
            instance = null;
        }

        private EntityGroupPlatforms()
        {
            // TODO:
            //PlatformDictionary = PlatformGroup.GetPlatformsDictonary(ModStrings.GROUP,
            //    SetupGroup.BlocksGroupA,
            //    SetupGroup.BlocksGroupB,
            //    SetupGroup.BlocksGroupC,
            //    SetupGroup.BlocksGroupD);
        }

        protected override void EntityUpdate(float p_delta)
        {
            int tick = AchievementManager.GetTicks();
            float multiplier = SettingsGroup.Multiplier;
            List<int> finished = new List<int>();
            Parallel.ForEach(DataGroup.Active, group =>
            {
                BlockGroup blockGroup = DataGroup.Groups[group];
                blockGroup.Progress = UpdateProgressClamped(
                    blockGroup.State,
                    blockGroup.Progress,
                    p_delta,
                    multiplier);
                TrySwitch(blockGroup, tick);
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
            foreach (int i in finished)
            {
                BlockGroup blockGroup = DataGroup.Groups[i];
                if (!blockGroup.State && blockGroup.Progress == 0.0f)
                {
                    DataGroup.Finished.Add(i);
                }
                DataGroup.Active.Remove(i);
            }
        }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(
                    spriteBatch,
                    DataGroup.GetState(drawable.GroupId),
                    DataGroup.GetProgress(drawable.GroupId));
            });
        }

        private void TrySwitch(BlockGroup group, int tick)
        {
            // A platform is solid if the activated tick is larger than the current tick.
            bool newState = group.ActivatedTick > tick;
            if (group.State != newState)
            {
                if (currentDrawables != null)
                {
                    ModSounds.GroupFlip?.PlayOneShot();
                }
                group.State = newState;
            }
        }
    }
}
