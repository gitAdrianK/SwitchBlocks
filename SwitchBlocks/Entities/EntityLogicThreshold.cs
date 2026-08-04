namespace SwitchBlocks.Entities
{
    using System;
    using Data;
    using Patches;
    using Settings;
    using Util;

    /// <summary>
    ///     Threshold logic entity.
    /// </summary>
    public class EntityLogicThreshold : EntityLogic<DataThreshold>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public EntityLogicThreshold(SettingsThreshold settings) : base(DataThreshold.Instance)
            => this.UpdateSettings(settings);

        /// <summary>Stat to check for.</summary>
        private Stat Stat { get; set; }

        /// <summary>Threshold for the stat check.</summary>
        private int Count { get; set; }

        /// <summary>If the state is forced to switch regardless of player intersection.</summary>
        private bool ForceSwitch { get; set; }

        /// <summary>
        ///     Updates the settings from the given settings.
        /// </summary>
        /// <param name="settings"><see cref="SettingsThreshold" />.</param>
        public void UpdateSettings(SettingsThreshold settings)
        {
            this.Multiplier = settings.Multiplier;

            this.Stat = settings.Stat;
            this.Count = settings.Count;
            this.ForceSwitch = settings.ForceSwitch;
        }


        /// <summary>
        ///     Updates progress, tries to play sounds and switch the state.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);
            this.TrySwitch();
        }

        /// <summary>
        ///     Tries to switch the state if it should do so.
        /// </summary>
        private void TrySwitch()
        {
            int stat;
            switch (this.Stat)
            {
                case Stat.Jumps:
                    stat = PatchAchievementManager.GetJumps();
                    break;
                case Stat.Falls:
                    stat = PatchAchievementManager.GetFalls();
                    break;
                case Stat.Time:
                    stat = PatchAchievementManager.GetTick();
                    break;
                case Stat.Session:
                    stat = PatchAchievementManager.GetSession();
                    break;
                case Stat.Victory:
                    return;
                default:
                    throw new ArgumentOutOfRangeException("Unknown stat: " + this.Stat);
            }

            var adjustedCount = stat - this.Data.ResetCount;
            var currState = adjustedCount > this.Count;
            if (this.Data.State == currState)
            {
                return;
            }

            if (this.Data.CanSwitchSafely && this.Data.SwitchOnceSafe)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.ThresholdFlip?.PlayOneShot();
                }

                this.Data.State = currState;
                this.Data.SwitchOnceSafe = false;
                return;
            }


            if (this.Data.CanSwitchSafely || this.ForceSwitch)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.ThresholdFlip?.PlayOneShot();
                }

                this.Data.State = currState;
            }
            else
            {
                this.Data.SwitchOnceSafe = true;
            }
        }
    }
}
