using EntityComponent;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Factories;
using System.Diagnostics;
using System.IO;

namespace SwitchBlocks
{
    [JumpKingMod(ModStrings.MODNAME)]
    public static class ModEntry
    {
        public static Harmony Harmony { get; set; }

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

            Harmony = new Harmony(ModStrings.HARMONY);
            new Patching.BodyComp();
            new Patching.EndingManager();
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}";

            // Level being null means the vanilla map is being started/ended.
            if (contentManager.level == null)
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                return;
            }

            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            ModBlocks.LoadProperties();
            ModSaves.Load();
            ModSounds.Load();

            if (ModBlocks.IsAutoUsed)
            {
                DoAutoSetup(entityManager, player);
            }

            if (ModBlocks.IsBasicUsed)
            {
                DoBasicSetup(entityManager, player);
            }

            if (ModBlocks.IsCountdownUsed)
            {
                DoCountdownSetup(entityManager, player);
            }

            if (ModBlocks.IsJumpUsed)
            {
                DoJumpSetup(entityManager);
            }

            if (ModBlocks.IsSandUsed)
            {
                DoSandSetup(entityManager, player);
            }

            EntityManager.instance.MoveToFront(player);
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}";

            // Level being null means the vanilla map is being started/ended.
            if (contentManager.level == null)
            {
                return;
            }
            if (!Directory.Exists(path))
            {
                return;
            }

            EntityManager entityManager = EntityManager.instance;
            if (ModBlocks.IsAutoUsed)
            {
                entityManager.RemoveObject(EntityAutoPlatforms.Instance);
                EntityAutoPlatforms.Instance.Reset();
            }

            if (ModBlocks.IsBasicUsed)
            {
                entityManager.RemoveObject(EntityBasicPlatforms.Instance);
                entityManager.RemoveObject(EntityBasicLevers.Instance);
                EntityBasicPlatforms.Instance.Reset();
                EntityBasicLevers.Instance.Reset();
            }

            if (ModBlocks.IsCountdownUsed)
            {
                entityManager.RemoveObject(EntityCountdownPlatforms.Instance);
                entityManager.RemoveObject(EntityCountdownLevers.Instance);
                EntityCountdownPlatforms.Instance.Reset();
                EntityCountdownLevers.Instance.Reset();
            }

            if (ModBlocks.IsJumpUsed)
            {
                entityManager.RemoveObject(EntityJumpPlatforms.Instance);
                EntityJumpPlatforms.Instance.Reset();
                PlayerEntity.OnJumpCall -= JumpSwitch;
            }

            if (ModBlocks.IsSandUsed)
            {
                entityManager.RemoveObject(EntitySandPlatforms.Instance);
                entityManager.RemoveObject(EntitySandLevers.Instance);
                EntitySandPlatforms.Instance.Reset();
                EntitySandLevers.Instance.Reset();
            }
            ModSaves.Save();
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

        private static void DoAutoSetup(EntityManager entityManager, PlayerEntity player)
        {
            Debugger.Log(1, "", ">>> Doing Auto Setup\n");
            entityManager.AddObject(EntityAutoPlatforms.Instance);
            BehaviourAutoReset behaviourAutoReset = new BehaviourAutoReset();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoReset), behaviourAutoReset);
        }

        private static void DoBasicSetup(EntityManager entityManager, PlayerEntity player)
        {
            Debugger.Log(1, "", ">>> Doing Basic Setup\n");
            entityManager.AddObject(EntityBasicPlatforms.Instance);
            entityManager.AddObject(EntityBasicLevers.Instance);
            BehaviourBasicLever behaviourBasicLever = new BehaviourBasicLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOff), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolid), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOff), behaviourBasicLever);
        }

        private static void DoCountdownSetup(EntityManager entityManager, PlayerEntity player)
        {
            Debugger.Log(1, "", ">>> Doing Countdown Setup\n");
            entityManager.AddObject(EntityCountdownPlatforms.Instance);
            entityManager.AddObject(EntityCountdownLevers.Instance);
            BehaviourCountdownLever behaviourCountdownLever = new BehaviourCountdownLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), behaviourCountdownLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLeverSolid), behaviourCountdownLever);
        }

        private static void DoJumpSetup(EntityManager entityManager)
        {
            Debugger.Log(1, "", ">>> Doing Jump Setup\n");
            entityManager.AddObject(EntityJumpPlatforms.Instance);
            PlayerEntity.OnJumpCall += JumpSwitch;
        }

        private static void DoSandSetup(EntityManager entityManager, PlayerEntity player)
        {
            Debugger.Log(1, "", ">>> Doing Sand Setup\n");
            entityManager.AddObject(EntitySandPlatforms.Instance);
            entityManager.AddObject(EntitySandLevers.Instance);
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
        }
    }
}