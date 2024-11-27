﻿using EntityComponent;
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
    /// Entity responsible for rendering group platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityGroupPlatforms : Entity
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

        public void Reset()
        {
            instance = null;
        }

        private EntityGroupPlatforms()
        {
            PlatformDictionary = PlatformGroup.GetPlatformsDictonary(ModStrings.GROUP,
                SetupGroup.BlocksGroupA,
                SetupGroup.BlocksGroupB,
                SetupGroup.BlocksGroupC,
                SetupGroup.BlocksGroupD);
        }

        // TODO: Inherit from EntityPlatforms and cut down on repeated code
        private int currentScreen = -1;
        private int nextScreen;

        public Dictionary<int, List<PlatformGroup>> PlatformDictionary { get; protected set; }
        private List<PlatformGroup> currentPlatformList;

        protected override void Update(float deltaTime)
        {
            int tick = AchievementManager.GetTicks();
            float multiplier = SettingsGroup.Multiplier;
            List<int> finished = new List<int>();
            Parallel.ForEach(DataGroup.Active, group =>
            {
                BlockGroup blockGroup = DataGroup.Groups[group];
                UpdateProgress(blockGroup, deltaTime, multiplier);
                TrySwitch(blockGroup, tick);
                if (blockGroup.Progress == Convert.ToInt32(blockGroup.State))
                {
                    lock (finished)
                    {
                        finished.Add(group);
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
                    DataGroup.GetProgress(platform.GroupId),
                    DataGroup.GetState(platform.GroupId),
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
                    ModSounds.GroupFlip?.PlayOneShot();
                }
                group.State = newState;
            }
        }
    }
}
