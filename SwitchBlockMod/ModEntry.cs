using EntityComponent;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using SwitchBlocksMod.Behaviours;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Entities;
using SwitchBlocksMod.Factories;
using System;
using System.Reflection;

namespace SwitchBlocksMod
{
    [JumpKingMod(ModStrings.MODNAME)]
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

            Harmony harmony = new Harmony(ModStrings.MODNAME);
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
            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            ModSaves.Load();
            ModSounds.Load();

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
            PlayerEntity.OnJumpCall += JumpSwitch;

            // Sand
            if (EntitySandPlatforms.Instance.PlatformDictionary != null
                && EntityCountdownLevers.Instance.LeverDictionary != null)
            {
                entityManager.AddObject(EntitySandPlatforms.Instance);
                entityManager.AddObject(EntitySandLevers.Instance);
            }
            BehaviourSandPlatform behaviourSandPlatform = new BehaviourSandPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSandPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSandPlatform);
            BehaviourSandLever behaviourSandLever = new BehaviourSandLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOn), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOff), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolid), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOn), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOff), behaviourSandLever);

            JKContentManager contentManager = Game1.instance.contentManager;
            if (contentManager != null)
            {
                player.RegisterLandSound<BlockSandOn>(contentManager.audio.player.SandLand);
                player.RegisterLandSound<BlockSandOff>(contentManager.audio.player.SandLand);
            }

            ModBlocks.LoadProperties();

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
            PlayerEntity.OnJumpCall -= JumpSwitch;
        }

        private static MethodInfo originalIsOnBlock;
        /// <summary>
        /// Function to be patched in with harmony, adds the custom sand blocks from this mod to also return true
        /// in the IsOnBlock function when asked if the player is on a sand block.
        /// </summary>
        /// <param name="__instance">Object instance of the body comp</param>
        /// <param name="__result">Result of the original function, returning true if the player is on a sand block</param>
        /// <param name="__0">Original object the function is called with</param>
        public static void IsOnBlockPostfix(object __instance, ref bool __result, Type __0)
        {
            if (__0 == typeof(SandBlock) && originalIsOnBlock != null)
            {
                __result = __result || BehaviourSandPlatform.HasEntered;
            }
        }

        private static void JumpSwitch()
        {
            if (EntityJumpPlatforms.Instance.PlatformDictionary != null
                && EntityJumpPlatforms.Instance.PlatformDictionary.ContainsKey(Camera.CurrentScreen))
            {
                ModSounds.jumpFlip?.Play();
            }
            DataJump.State = !DataJump.State;
        }
    }
}