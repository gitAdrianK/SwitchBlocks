﻿using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using SwitchBlocks.Settings;
using SwitchBlocks.Setups;
using SwitchBlocks.Util;
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
                currentPlatformList = null;
                if (PlatformDictionary.ContainsKey(nextScreen))
                {
                    currentPlatformList = PlatformDictionary[nextScreen];
                }
                currentScreen = nextScreen;
            }

            if (currentPlatformList == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="group">Blockgroup with progress and state</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        protected void UpdateProgress(BlockGroup group, float amount, float multiplier)
        {
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= 2.0f;
            amount *= multiplier;
            if (group.Progress != 1.0f && group.State)
            {
                group.Progress += amount;
                if (group.Progress >= 1.0f)
                {
                    group.Progress = 1.0f;
                }
            }
            else if (group.Progress != 0.0f && !group.State)
            {
                group.Progress -= amount;
                if (group.Progress <= 0.0f)
                {
                    group.Progress = 0.0f;
                }
            }
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