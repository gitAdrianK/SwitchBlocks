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
            int adjustedTick = currentTick - DataAuto.ResetTick;
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
            int soundAdjust = (SettingsAuto.WarnCount - DataAuto.WarnCount) * SettingsAuto.WarnDuration;
            int soundTick = (adjustedTick + soundAdjust) % SettingsAuto.Duration;
            if (soundTick != 0)
            {
                return;
            }
            if (SettingsAuto.WarnCount == DataAuto.WarnCount)
            {
                if (IsActiveOnCurrentScreen)
                {
                    ModSounds.AutoFlip?.PlayOneShot();
                }
                DataAuto.WarnCount = 0;
            }
            else
            {
                if (IsActiveOnCurrentScreen)
                {
                    ModSounds.AutoWarn?.PlayOneShot();
                }
                DataAuto.WarnCount++;
            }
        }

        private void TrySwitch(int adjustedTick)
        {
            bool currState = adjustedTick / SettingsAuto.Duration % 2 == 0;
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
