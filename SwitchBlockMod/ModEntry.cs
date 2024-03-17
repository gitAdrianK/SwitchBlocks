using EntityComponent;
using Harmony;
using JumpKing.API;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using Microsoft.Xna.Framework;
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
                LevelManager.RegisterBlockFactory((IBlockFactory)constructorInfo.Invoke(null));
            };

            var harmony = HarmonyInstance.Create("Zebra.SwitchBlocksMod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            MethodInfo isOnBlockMethod = typeof(BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });
            MethodInfo postfixMethod = typeof(ModEntry).GetMethod("IsOnBlockPostfix");
            originalIsOnBlock = harmony.Patch(isOnBlockMethod);
            harmony.Patch(isOnBlockMethod, postfix: new HarmonyMethod(postfixMethod));
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
            ModSaves.Load();
            ModSounds.Load();
            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            // Auto
            if (ModBlocks.IS_AUTO_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityAutoPlatforms.Instance.PlatformDictionary.Count > 0)
                {
                    entityManager.AddObject(EntityAutoPlatforms.Instance);
                }
            }
            // Basic
            if (ModBlocks.IS_BASIC_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityBasicPlatforms.Instance.PlatformDictionary.Count > 0
                    && EntityBasicLevers.Instance.LeverDictionary.Count > 0)
                {
                    entityManager.AddObject(EntityBasicPlatforms.Instance);
                    entityManager.AddObject(EntityBasicLevers.Instance);
                }
                BehaviourBasicLever behaviourBasicLever = new BehaviourBasicLever();
                List<(Color?, Type, IBlockBehaviour)> blocks = new List<(Color?, Type, IBlockBehaviour)>
                {
                    (ModBlocks.BASIC_LEVER, typeof(BlockBasicLever), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_ON, typeof(BlockBasicLeverOn), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_OFF, typeof(BlockBasicLeverOff), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_SOLID, typeof(BlockBasicLeverSolid), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_SOLID_ON, typeof(BlockBasicLeverSolidOn), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_SOLID_OFF, typeof(BlockBasicLeverSolidOff), behaviourBasicLever),
                };
                foreach (var item in blocks.Where(triple => triple.Item1 != null))
                {
                    player.m_body.RegisterBlockBehaviour(item.Item2, item.Item3);
                }
            }
            // Countdown
            if (ModBlocks.IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityCountdownPlatforms.Instance.PlatformDictionary.Count > 0
                    && EntityCountdownLevers.Instance.LeverDictionary.Count > 0)
                {
                    entityManager.AddObject(EntityCountdownPlatforms.Instance);
                    entityManager.AddObject(EntityCountdownLevers.Instance);
                }
                BehaviourCountdownLever behaviourCountdownLever = new BehaviourCountdownLever();
                List<(Color?, Type, IBlockBehaviour)> blocks = new List<(Color?, Type, IBlockBehaviour)>
                {
                    (ModBlocks.COUNTDOWN_LEVER, typeof(BlockCountdownLever), behaviourCountdownLever),
                    (ModBlocks.COUNTDOWN_LEVER_SOLID, typeof(BlockCountdownLeverSolid), behaviourCountdownLever),
                };
                foreach (var item in blocks.Where(triple => triple.Item1 != null))
                {
                    player.m_body.RegisterBlockBehaviour(item.Item2, item.Item3);
                }
            }
            // Sand
            if (ModBlocks.IS_SAND_FUNCTIONALLY_INITIALIZED)
            {
                Debugger.Log(1, "", ">>> Sand Platforms "
                        + EntitySandPlatforms.Instance.PlatformDictionary.Count
                        + ", Levers "
                        + EntitySandLevers.Instance.LeverDictionary.Count
                        + "\n");
                if (EntitySandPlatforms.Instance.PlatformDictionary.Count > 0
                    && EntitySandLevers.Instance.LeverDictionary.Count > 0)
                {
                    Debugger.Log(1, "", "Yeah\n");
                    entityManager.AddObject(EntitySandPlatforms.Instance);
                    entityManager.AddObject(EntitySandLevers.Instance);
                }
                BehaviourSand behaviourSand = new BehaviourSand();
                BehaviourSandLever behaviourSandLever = new BehaviourSandLever();
                List<(Color?, Type, IBlockBehaviour)> blocks = new List<(Color?, Type, IBlockBehaviour)>
                {
                    (ModBlocks.SAND_ON, typeof(BlockSandOn), behaviourSand),
                    (ModBlocks.SAND_OFF, typeof(BlockSandOff), behaviourSand),
                    (ModBlocks.SAND_LEVER, typeof(BlockSandLever), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_ON, typeof(BlockSandLeverOn), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_OFF, typeof(BlockSandLeverOff), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_SOLID, typeof(BlockSandLeverSolid), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_SOLID_ON, typeof(BlockSandLeverSolidOn), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_SOLID_OFF, typeof(BlockSandLeverSolidOff), behaviourSandLever),
                };
                foreach (var item in blocks.Where(triple => triple.Item1 != null))
                {
                    player.m_body.RegisterBlockBehaviour(item.Item2, item.Item3);
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
            EntityManager entityManager = EntityManager.instance;
            entityManager.RemoveObject(EntityAutoPlatforms.Instance);
            entityManager.RemoveObject(EntityBasicPlatforms.Instance);
            entityManager.RemoveObject(EntityBasicLevers.Instance);
            entityManager.RemoveObject(EntityCountdownPlatforms.Instance);
            entityManager.RemoveObject(EntityCountdownLevers.Instance);
            entityManager.RemoveObject(EntitySandPlatforms.Instance);
            entityManager.RemoveObject(EntitySandLevers.Instance);
            EntityAutoPlatforms.Instance.Reset();
            EntityBasicPlatforms.Instance.Reset();
            EntityBasicLevers.Instance.Reset();
            EntityCountdownPlatforms.Instance.Reset();
            EntityCountdownLevers.Instance.Reset();
            EntitySandPlatforms.Instance.Reset();
            EntitySandLevers.Instance.Reset();
            ModSounds.Reset();
            ModSaves.Save();
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
