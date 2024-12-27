namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EntityComponent;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Platforms;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Setups;
    using SwitchBlocks.Util;

    /// <summary>
    /// Entity responsible for rendering sequence platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntitySequencePlatforms : Entity
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

        public void Reset() => instance = null;

        private EntitySequencePlatforms()
            => this.PlatformDictionary = PlatformGroup.GetPlatformsDictonary(ModStrings.SEQUENCE,
                SetupSequence.BlocksSequenceA,
                SetupSequence.BlocksSequenceB,
                SetupSequence.BlocksSequenceC,
                SetupSequence.BlocksSequenceD);

        private int currentScreen = -1;
        private int nextScreen;

        public Dictionary<int, List<PlatformGroup>> PlatformDictionary { get; protected set; }
        private List<PlatformGroup> currentPlatformList;

        protected override void Update(float deltaTime)
        {
            var tick = AchievementManager.GetTicks();
            var multiplier = SettingsSequence.Multiplier;
            var finished = new List<int>();
            _ = Parallel.ForEach(DataSequence.Active, group =>
            {
                var blockGroup = DataSequence.Groups[group];
                this.UpdateProgress(blockGroup, deltaTime, multiplier);
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

        public override void Draw()
        {
            if (!this.UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            var spriteBatch = Game1.spriteBatch;
            _ = Parallel.ForEach(this.currentPlatformList, platform
                => EntityPlatforms.DrawPlatform(platform,
                    DataSequence.GetProgress(platform.GroupId),
                    DataSequence.GetState(platform.GroupId),
                    spriteBatch));
        }

        /// <summary>
        /// Updates what screen is currently active and gets the platforms from the platform dictionary
        /// </summary>
        /// <returns>false if no platforms are to be drawn, true otherwise</returns>
        protected bool UpdateCurrentScreen()
        {
            if (this.PlatformDictionary == null)
            {
                return false;
            }

            this.nextScreen = Camera.CurrentScreen;
            if (this.currentScreen != this.nextScreen)
            {
                _ = this.PlatformDictionary.TryGetValue(this.nextScreen, out this.currentPlatformList);
                this.currentScreen = this.nextScreen;
            }
            return this.currentPlatformList != null;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="group">Blockgroup with progress and state</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        protected void UpdateProgress(BlockGroup group, float amount, float multiplier)
        {
            var stateInt = Convert.ToInt32(group.State);
            if (group.Progress == stateInt)
            {
                return;
            }
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= (-1 + (stateInt * 2)) * 2 * multiplier;
            group.Progress += amount;
            group.Progress = Math.Min(Math.Max(group.Progress, 0), 1);
        }

        private void TrySwitch(BlockGroup group, int tick)
        {
            // A platform is solid if the activated tick is larger than the current tick.
            var newState = group.ActivatedTick > tick;
            if (group.State != newState)
            {
                if (this.currentPlatformList != null)
                {
                    ModSounds.SequenceFlip?.PlayOneShot();
                }
                group.State = newState;
            }
        }
    }
}
