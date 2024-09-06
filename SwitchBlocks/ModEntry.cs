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
                DoAutoSetup(player);
            }

            if (ModBlocks.IsBasicUsed)
            {
                DoBasicSetup(player);
            }

            if (ModBlocks.IsCountdownUsed)
            {
                DoCountdownSetup(player);
            }

            if (ModBlocks.IsJumpUsed)
            {
                DoJumpSetup(player);
            }

            if (ModBlocks.IsSandUsed)
            {
                DoSandSetup(player);
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
                ModSounds.JumpFlip?.PlayOneShot();
            }
            DataJump.State = !DataJump.State;
        }

        private static void DoAutoSetup(PlayerEntity player)
        {
            _ = EntityAutoPlatforms.Instance;

            BehaviourAutoPlatform behaviourAutoPlatform = new BehaviourAutoPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOn), behaviourAutoPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOff), behaviourAutoPlatform);

            BehaviourAutoIceOn behaviourAutoIceOn = new BehaviourAutoIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoIceOn), behaviourAutoIceOn);
            BehaviourAutoIceOff behaviourAutoIceOff = new BehaviourAutoIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoIceOff), behaviourAutoIceOff);

            BehaviourAutoReset behaviourAutoReset = new BehaviourAutoReset();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoReset), behaviourAutoReset);
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoResetFull), behaviourAutoReset);
        }

        private static void DoBasicSetup(PlayerEntity player)
        {
            _ = EntityBasicPlatforms.Instance;
            _ = EntityBasicLevers.Instance;

            BehaviourBasicIceOn behaviourBasicIceOn = new BehaviourBasicIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicIceOn), behaviourBasicIceOn);
            BehaviourBasicIceOff behaviourBasicIceOff = new BehaviourBasicIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicIceOff), behaviourBasicIceOff);

            BehaviourBasicLever behaviourBasicLever = new BehaviourBasicLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOff), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolid), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOff), behaviourBasicLever);
        }

        private static void DoCountdownSetup(PlayerEntity player)
        {
            _ = EntityCountdownPlatforms.Instance;
            _ = EntityCountdownLevers.Instance;

            BehaviourCountdownPlatform behaviourCountdownPlatform = new BehaviourCountdownPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOn), behaviourCountdownPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOff), behaviourCountdownPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOn), behaviourCountdownPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOff), behaviourCountdownPlatform);

            BehaviourCountdownIceOn behaviourCountdownIceOn = new BehaviourCountdownIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOn), behaviourCountdownIceOn);
            BehaviourCountdownIceOff behaviourCountdownIceOff = new BehaviourCountdownIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOff), behaviourCountdownIceOff);

            BehaviourCountdownLever behaviourCountdownLever = new BehaviourCountdownLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), behaviourCountdownLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLeverSolid), behaviourCountdownLever);
        }

        private static void DoJumpSetup(PlayerEntity player)
        {
            _ = EntityJumpPlatforms.Instance;

            BehaviourJumpIceOn behaviourJumpIceOn = new BehaviourJumpIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpIceOn), behaviourJumpIceOn);
            BehaviourJumpIceOff behaviourJumpIceOff = new BehaviourJumpIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpIceOff), behaviourJumpIceOff);

            PlayerEntity.OnJumpCall += JumpSwitch;
        }

        private static void DoSandSetup(PlayerEntity player)
        {
            _ = EntitySandPlatforms.Instance;
            _ = EntitySandLevers.Instance;

            // XXX: Do not register the same behaviour for multiple blocks if the behaviour changes
            // velocity or position! This technically needs updating, but I have to consider
            // Ghost of the Immortal Babe breaking!
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
