using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using SwitchBlocks.Settings;
using SwitchBlocks.Setups;
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

        public void Reset()
        {
            instance = null;
        }

        private EntitySequencePlatforms()
        {
            PlatformDictionary = PlatformGroup.GetPlatformsDictonary(ModStrings.SEQUENCE,
                SetupSequence.BlocksSequenceA,
                SetupSequence.BlocksSequenceB,
                SetupSequence.BlocksSequenceC,
                SetupSequence.BlocksSequenceD);
        }

        private int currentScreen = -1;
        private int nextScreen;

        public Dictionary<int, List<PlatformGroup>> PlatformDictionary { get; protected set; }
        private List<PlatformGroup> currentPlatformList;

        protected override void Update(float deltaTime)
        {
            //TODO: Optimize just like Group to not update all groups, but only those considered "active"
            int tick = AchievementManager.GetTicks();
            float multiplier = SettingsSequence.Multiplier;
            Parallel.ForEach(DataSequence.Groups.Values, group =>
            {
                UpdateProgress(group, deltaTime, multiplier);
                TrySwitch(group, tick);
            });
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
                EntityPlatforms.DrawPlatform(platform,
                    DataSequence.GetProgress(platform.GroupId),
                    DataSequence.GetState(platform.GroupId),
                    spriteBatch);
            });
        }

        /// <summary>
        /// Updates what screen is currently active and gets the platforms from the platform dictionary
        /// </summary>
        /// <returns>false if no platforms are to be drawn, true otherwise</returns>
        protected bool UpdateCurrentScreen()
        {
            if (PlatformDictionary == null)
            {
                return false;
            }

            nextScreen = Camera.CurrentScreen;
            if (currentScreen != nextScreen)
            {
                PlatformDictionary.TryGetValue(nextScreen, out currentPlatformList);
                currentScreen = nextScreen;
            }
            return currentPlatformList != null;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="group">Blockgroup with progress and state</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        protected void UpdateProgress(BlockGroup group, float amount, float multiplier)
        {
            int stateInt = Convert.ToInt32(group.State);
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
            bool newState = group.ActivatedTick > tick;
            if (group.State != newState)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.SequenceFlip?.PlayOneShot();
                }
                group.State = newState;
            }
        }
    }
}
