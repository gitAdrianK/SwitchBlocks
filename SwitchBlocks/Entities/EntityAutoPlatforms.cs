using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using SwitchBlocks.Settings;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering auto platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityAutoPlatforms : EntityPlatforms
    {
        private static EntityAutoPlatforms instance;
        public static EntityAutoPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityAutoPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            DataAuto.Progress = progress;
            instance = null;
        }

        private EntityAutoPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.AUTO);
            progress = DataAuto.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataAuto.State, deltaTime, SettingsAuto.Multiplier);

            int currentTick = AchievementManager.GetTicks();
            int adjustedTick = ((currentTick + SettingsAuto.DurationCycle) - DataAuto.ResetTick) % SettingsAuto.DurationCycle;
            TrySound(adjustedTick);
            TrySwitch(adjustedTick);
        }

        public override void Draw()
        {
            if (!UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            SpriteBatch spriteBatch = Game1.spriteBatch;
            Parallel.ForEach(currentPlatformList, platform =>
            {
                DrawPlatform(platform, progress, DataAuto.State, spriteBatch);
            });
        }

        private void TrySound(int adjustedTick)
        {
            if (DataAuto.State)
            {
                adjustedTick += SettingsAuto.DurationOff;
            }
            int soundAdjust = (SettingsAuto.WarnCount - DataAuto.WarnCount) * SettingsAuto.WarnDuration;
            int soundTick = (adjustedTick + soundAdjust) % SettingsAuto.DurationCycle;
            // Its not yet time to make a sound
            if (soundTick != 0)
            {
                return;
            }
            // Check which sound is to be played
            if (SettingsAuto.WarnCount == DataAuto.WarnCount)
            {
                DoFlipSound();
            }
            else
            {
                DoWarnSound();
            }
        }

        private void DoWarnSound()
        {
            DataAuto.WarnCount++;
            if (!IsActiveOnCurrentScreen)
            {
                return;
            }
            // The sound was disabled
            if (DataAuto.State)
            {
                if (SettingsAuto.WarnDisableOn)
                {
                    return;
                }
            }
            else
            {
                if (SettingsAuto.WarnDisableOff)
                {
                    return;
                }
            }
            ModSounds.AutoWarn?.PlayOneShot();
        }

        private void DoFlipSound()
        {
            DataAuto.WarnCount = 0;
            if (!IsActiveOnCurrentScreen)
            {
                return;
            }
            ModSounds.AutoFlip?.PlayOneShot();
        }

        private void TrySwitch(int adjustedTick)
        {
            // I think its < but it could be <=
            bool currState = adjustedTick - SettingsAuto.DurationOn < 0;
            if (DataAuto.State == currState)
            {
                return;
            }

            if (DataAuto.CanSwitchSafely && DataAuto.SwitchOnceSafe)
            {
                DataAuto.State = currState;
                DataAuto.SwitchOnceSafe = false;
                return;
            }


            if (DataAuto.CanSwitchSafely || SettingsAuto.ForceSwitch)
            {
                DataAuto.State = currState;
            }
            else
            {
                DataAuto.SwitchOnceSafe = true;
            }
        }
    }
}
