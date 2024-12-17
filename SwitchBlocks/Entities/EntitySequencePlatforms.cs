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
    /// Entity responsible for rendering sequence platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntitySequencePlatforms : EntityDrawables<PlatformInOutGroup>
    {
        private static EntitySequencePlatforms instance;
        public static EntitySequencePlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntitySequencePlatforms();
                }
                return instance;
            }
        }

        private EntitySequencePlatforms()
        {
            // TODO:
            //PlatformDictionary = PlatformGroup.GetPlatformsDictonary(ModStrings.SEQUENCE,
            //    SetupSequence.BlocksSequenceA,
            //    SetupSequence.BlocksSequenceB,
            //    SetupSequence.BlocksSequenceC,
            //    SetupSequence.BlocksSequenceD);
        }

        public override void Reset()
        {
            instance.Destroy();
            instance = null;
        }

        protected override void EntityUpdate(float p_delta)
        {
            int tick = AchievementManager.GetTicks();
            float multiplier = SettingsSequence.Multiplier;
            List<int> finished = new List<int>();
            Parallel.ForEach(DataSequence.Active, group =>
            {
                BlockGroup blockGroup = DataSequence.Groups[group];
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
                    if (!(tick <= blockGroup.ActivatedTick && tick + SettingsSequence.Duration >= blockGroup.ActivatedTick))
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
                BlockGroup blockGroup = DataSequence.Groups[i];
                if (blockGroup.State && blockGroup.Progress == 1.0f)
                {
                    DataSequence.Finished.Add(i);
                }
                DataSequence.Active.Remove(i);
            }
        }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(
                    spriteBatch,
                    DataSequence.GetState(drawable.GroupId),
                    DataSequence.GetProgress(drawable.GroupId));
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
                    ModSounds.SequenceFlip?.PlayOneShot();
                }
                group.State = newState;
            }
        }
    }
}
