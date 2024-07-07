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
            new Patching.BodyComp(harmony);
            new Patching.EndingManager(harmony);
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
                || EntityBasicLevers.Instance.LeverDictionary != null)
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
                || EntityCountdownLevers.Instance.LeverDictionary != null)
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
                || EntityCountdownLevers.Instance.LeverDictionary != null)
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

        private static void JumpSwitch()
        {
            if (EntityJumpPlatforms.Instance.PlatformDictionary != null
                && EntityJumpPlatforms.Instance.PlatformDictionary.ContainsKey(Camera.CurrentScreen))
            {
                ModSounds.jumpFlip?.PlayOneShot();
            }
            DataJump.State = !DataJump.State;
        }
    }
}