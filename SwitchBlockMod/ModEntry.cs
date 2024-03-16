using EntityComponent;
using Harmony;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using SwitchBlocksMod.Behaviours;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Entities;
using SwitchBlocksMod.Factories;
using SwitchBlocksMod.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SwitchBlocksMod
{
    [JumpKingMod("Zebra.SwitchBlocksMod")]
    public static class ModEntry
    {
        /// <summary>
        /// Called by Jump King before the level loads
        /// -> OnGameStart
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            Debugger.Launch();

            List<(bool, Type)> blockFactories = new List<(bool, Type)>
            {
                (ModBlocks.IS_AUTO_FUNCTIONALLY_INITIALIZED, typeof(FactoryAuto)),
                (ModBlocks.IS_BASIC_FUNCTIONALLY_INITIALIZED, typeof(FactoryBasic)),
                (ModBlocks.IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED, typeof(FactoryCountdown)),
                (ModBlocks.IS_SAND_FUNCTIONALLY_INITIALIZED, typeof(FactorySand)),
            };
            foreach (var item in blockFactories.Where(pair => pair.Item1))
            {
                var constructorInfo = item.Item2.GetConstructor(Type.EmptyTypes);
                LevelManager.RegisterBlockFactory((JumpKing.API.IBlockFactory)constructorInfo.Invoke(null));
            };
        }

        /// <summary>
        /// Called by Jump King when the level unloads
        /// -> OnGameEnd
        /// </summary>
        [OnLevelUnload]
        public static void OnLevelUnload()
        {
            // Your code here
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            //TODO_LO: "Cleanup" like BeforeLevelLoad()
            ModSaves.LoadData();
            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            // Auto
            if (ModBlocks.IS_AUTO_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityAutoPlatforms.Instance.PlatformDictionary.Count != 0)
                {
                    entityManager.AddObject(EntityAutoPlatforms.Instance);
                }
                else
                {
                    EntityAutoPlatforms.Instance.Dispose();
                }
            }
            // Basic
            if (ModBlocks.IS_BASIC_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityBasicPlatforms.Instance.PlatformDictionary.Count != 0
                    && EntityBasicLevers.Instance.LeverDictionary.Count != 0)
                {
                    entityManager.AddObject(EntityBasicPlatforms.Instance);
                    entityManager.AddObject(EntityBasicLevers.Instance);
                }
                else
                {
                    EntityBasicPlatforms.Instance.Dispose();
                    EntityBasicLevers.Instance.Dispose();
                }
                BehaviourBasicLever behaviourBasicLever = new BehaviourBasicLever();
                if (ModBlocks.BASIC_LEVER != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), behaviourBasicLever);
                }
                if (ModBlocks.BASIC_LEVER_ON != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOn), behaviourBasicLever);
                }
                if (ModBlocks.BASIC_LEVER_OFF != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOff), behaviourBasicLever);
                }
                if (ModBlocks.BASIC_LEVER_SOLID != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolid), behaviourBasicLever);
                }
                if (ModBlocks.BASIC_LEVER_SOLID_ON != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOn), behaviourBasicLever);
                }
                if (ModBlocks.BASIC_LEVER_SOLID_OFF != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOff), behaviourBasicLever);
                }
            }
            // Countdown
            if (ModBlocks.IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityCountdownPlatforms.Instance.PlatformDictionary.Count != 0
                    && EntityCountdownLevers.Instance.LeverDictionary.Count != 0)
                {

                    entityManager.AddObject(EntityCountdownPlatforms.Instance);
                    entityManager.AddObject(EntityCountdownLevers.Instance);
                }
                else
                {
                    EntityCountdownPlatforms.Instance.Dispose();
                    EntityCountdownLevers.Instance.Dispose();
                }
                BehaviourCountdownLever behaviourCountdownLever = new BehaviourCountdownLever();
                if (ModBlocks.COUNTDOWN_LEVER != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), behaviourCountdownLever);
                }
                if (ModBlocks.COUNTDOWN_LEVER_SOLID != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLeverSolid), behaviourCountdownLever);
                }
            }
            // Sand
            if (ModBlocks.IS_SAND_FUNCTIONALLY_INITIALIZED)
            {
                if (EntitySandPlatforms.Instance.PlatformDictionary.Count != 0
                    && EntitySandLevers.Instance.LeverDictionary.Count != 0)
                {
                    entityManager.AddObject(EntitySandPlatforms.Instance);
                    entityManager.AddObject(EntitySandLevers.Instance);
                }
                else
                {
                    EntitySandPlatforms.Instance.Dispose();
                    EntitySandLevers.Instance.Dispose();
                }

                // TODO_LO: Move into more sensible place (probably BehaviourSand)
                var harmony = HarmonyInstance.Create("Zebra.SwitchBlocksMod");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                MethodInfo isOnBlockMethod = typeof(BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });
                MethodInfo postfixMethod = typeof(ModEntry).GetMethod("IsOnBlockPostfix");
                originalIsOnBlock = harmony.Patch(isOnBlockMethod);
                harmony.Patch(isOnBlockMethod, postfix: new HarmonyMethod(postfixMethod));

                BehaviourSand behaviourSand = new BehaviourSand();
                if (ModBlocks.SAND_ON != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSand);
                }
                if (ModBlocks.SAND_OFF != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSand);
                }
                BehaviourSandLever behaviourSandLever = new BehaviourSandLever();
                if (ModBlocks.SAND_LEVER != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), behaviourSandLever);
                }
                if (ModBlocks.SAND_LEVER_ON != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOn), behaviourSandLever);
                }
                if (ModBlocks.SAND_LEVER_OFF != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOff), behaviourSandLever);
                }
                if (ModBlocks.SAND_LEVER_SOLID != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolid), behaviourSandLever);
                }
                if (ModBlocks.SAND_LEVER_SOLID_ON != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOn), behaviourSandLever);
                }
                if (ModBlocks.SAND_LEVER_SOLID_OFF != null)
                {
                    player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOff), behaviourSandLever);
                }
            }
            // End
            EntityManager.instance.MoveToFront(player);
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            ModSaves.SaveData();
        }

        private static MethodInfo originalIsOnBlock;
        public static void IsOnBlockPostfix(object __instance, ref bool __result, Type __0)
        {
            if (__0 == typeof(SandBlock) && originalIsOnBlock != null)
            {
                __result = (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(SandBlock) })
                    || (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(BlockSandOn) })
                    || (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(BlockSandOff) });
            }
        }
    }
}
