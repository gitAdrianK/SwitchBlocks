﻿using EntityComponent;
using JumpKing;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SwitchBlocks.Data.DataGroup;

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
            PlatformDictionary = PlatformGroup.GetPlatformsDictonary(ModStrings.GROUP);
        }

        private int currentScreen = -1;
        private int nextScreen;

        public Dictionary<int, List<PlatformGroup>> PlatformDictionary { get; protected set; }
        private List<PlatformGroup> currentPlatformList;

        protected override void Update(float deltaTime)
        {
            int tick = AchievementManager.GetTicks();
            float multiplier = ModBlocks.GroupMultiplier;
            Parallel.ForEach(DataGroup.Groups.Values, group =>
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

            // TODO: Draw platforms
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
        /// <param name="group">Group with progress and state</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        protected void UpdateProgress(Group group, float amount, float multiplier)
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

        private void TrySwitch(Group group, int tick)
        {
            // TODO: Maybe switching sounds
            // A platform is solid if the activated tick is larger than the current tick.
            bool newState = group.ActivatedTick > tick;
            if (group.State != newState)
            {
                group.State = newState;
            }
        }
    }
}