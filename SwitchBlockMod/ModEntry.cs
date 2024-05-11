using EntityComponent;
using HarmonyLib;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using SwitchBlocksMod.Behaviours;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Entities;
using SwitchBlocksMod.Factories;
using SwitchBlocksMod.Util;
using System;
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
            //Debugger.Launch();

            LevelManager.RegisterBlockFactory(new FactoryAuto());
            LevelManager.RegisterBlockFactory(new FactoryBasic());
            LevelManager.RegisterBlockFactory(new FactoryCountdown());
            LevelManager.RegisterBlockFactory(new FactoryJump());
            LevelManager.RegisterBlockFactory(new FactorySand());

            var harmony = new Harmony("Zebra.SwitchBlocksMod");
            MethodInfo isOnBlockMethod = typeof(BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });
            MethodInfo postfixMethod = typeof(ModEntry).GetMethod("IsOnBlockPostfix");
            originalIsOnBlock = harmony.Patch(isOnBlockMethod);
            harmony.Patch(isOnBlockMethod, postfix: new HarmonyMethod(postfixMethod));
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            ModSaves.Load();
            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            // Auto
            if (EntityAutoPlatforms.Instance.PlatformDictionary != null)
            {
                entityManager.AddObject(EntityAutoPlatforms.Instance);
            }

            // Basic
            if (EntityBasicPlatforms.Instance.PlatformDictionary != null
                && EntityBasicLevers.Instance.LeverDictionary != null)
            {
                entityManager.AddObject(EntityBasicPlatforms.Instance);
                entityManager.AddObject(EntityBasicLevers.Instance);
            }
            BehaviourBasicLever behaviourBasicLever = new BehaviourBasicLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOff), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolid), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOff), behaviourBasicLever);

            // Countdown
            if (EntityCountdownPlatforms.Instance.PlatformDictionary != null
                && EntityCountdownLevers.Instance.LeverDictionary != null)
            {
                entityManager.AddObject(EntityCountdownPlatforms.Instance);
                entityManager.AddObject(EntityCountdownLevers.Instance);
            }
            BehaviourCountdownLever behaviourCountdownLever = new BehaviourCountdownLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), behaviourCountdownLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLeverSolid), behaviourCountdownLever);

            // Jump
            if (EntityJumpPlatforms.Instance.PlatformDictionary != null)
            {
                entityManager.AddObject(EntityJumpPlatforms.Instance);
            }
            PlayerEntity.OnJumpCall += jumpswitch;

            // Sand
            if (EntitySandPlatforms.Instance.PlatformDictionary != null
                && EntityCountdownLevers.Instance.LeverDictionary != null)
            {
                entityManager.AddObject(EntitySandPlatforms.Instance);
                entityManager.AddObject(EntitySandLevers.Instance);
            }
            BehaviourSandPlatform behaviourSand = new BehaviourSandPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSand);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSand);
            BehaviourSandLever behaviourSandLever = new BehaviourSandLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOff), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolid), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOff), behaviourBasicLever);

            ModBlocks.LoadDuration();

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
            entityManager.RemoveObject(EntityJumpPlatforms.Instance);
            entityManager.RemoveObject(EntitySandPlatforms.Instance);
            entityManager.RemoveObject(EntitySandLevers.Instance);
            EntityAutoPlatforms.Instance.Reset();
            EntityBasicPlatforms.Instance.Reset();
            EntityBasicLevers.Instance.Reset();
            EntityCountdownPlatforms.Instance.Reset();
            EntityCountdownLevers.Instance.Reset();
            EntityJumpPlatforms.Instance.Reset();
            EntitySandPlatforms.Instance.Reset();
            EntitySandLevers.Instance.Reset();
            ModSaves.Save();

            // for jumpswitch blocks
            PlayerEntity.OnJumpCall -= jumpswitch;
        }

        private static MethodInfo originalIsOnBlock;
        /// <summary>
        /// Function to be patched in with harmony, adds the custom sand blocks from this mod to also return true
        /// in the IsOnBlock function when asked if the player is on a sand block.
        /// </summary>
        /// <param name="__instance">Object instance of the body comp</param>
        /// <param name="__result">Result of the patched function, returning true if the player is on any sand block</param>
        /// <param name="__0">Original object the function is called with</param>
        public static void IsOnBlockPostfix(object __instance, ref bool __result, Type __0)
        {
            if (__0 == typeof(SandBlock) && originalIsOnBlock != null)
            {
                __result = (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(SandBlock) })
                    || (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(BlockSandOn) })
                    || (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(BlockSandOff) });
            }
        }
        private static void jumpswitch()
        {
            DataJump.State = !DataJump.State;
        }
    }
}