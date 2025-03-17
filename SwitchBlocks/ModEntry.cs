namespace SwitchBlocks
{
    using System.Diagnostics;
    using System.Linq;
    using EntityComponent;
    using HarmonyLib;
    using JumpKing;
    using JumpKing.Level;
    using JumpKing.Mods;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;
    using SwitchBlocks.Setups;

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
            _ = Debugger.Launch();

            _ = LevelManager.RegisterBlockFactory(new FactoryAuto());
            _ = LevelManager.RegisterBlockFactory(new FactoryBasic());
            _ = LevelManager.RegisterBlockFactory(new FactoryCountdown());
            _ = LevelManager.RegisterBlockFactory(new FactoryGroup());
            _ = LevelManager.RegisterBlockFactory(new FactoryJump());
            _ = LevelManager.RegisterBlockFactory(new FactorySand());
            _ = LevelManager.RegisterBlockFactory(new FactorySequence());

            var harmony = new Harmony(ModStrings.HARMONY);
            _ = new Patching.BodyComp(harmony);
            _ = new Patching.EndingManager(harmony);
            _ = new Patching.WindManager(harmony);
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            var contentManager = Game1.instance.contentManager;
            if (contentManager.level == null)
            {
                return;
            }

            var levelID = contentManager.level.ID;
            SetupAuto.IsUsed = levelID == FactoryAuto.LastUsedMapId;
            SetupBasic.IsUsed = levelID == FactoryBasic.LastUsedMapId;
            SetupCountdown.IsUsed = levelID == FactoryCountdown.LastUsedMapId;
            SetupGroup.IsUsed = levelID == FactoryGroup.LastUsedMapId;
            SetupJump.IsUsed = levelID == FactoryJump.LastUsedMapId;
            SetupSand.IsUsed = levelID == FactorySand.LastUsedMapId;
            SetupSequence.IsUsed = levelID == FactorySequence.LastUsedMapId;

            if (!SetupAuto.IsUsed
                && !SetupBasic.IsUsed
                && !SetupCountdown.IsUsed
                && !SetupGroup.IsUsed
                && !SetupJump.IsUsed
                && !SetupSand.IsUsed
                && !SetupSequence.IsUsed)
            {
                return;
            }

            var entityManager = EntityManager.instance;
            var player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            ModSettings.Load();

            ModSounds.Load();

            // These behaviours are used as a way to create pre and post behaviour points
            // Mainly used to unify snow and ice behaviour, esp. ice behaviour since we don't
            // want to run the sliding function multiple times.
            _ = player.m_body.RegisterBlockBehaviour<BlockPre>(new BehaviourPre());
            _ = player.m_body.RegisterBlockBehaviour<BlockPost>(new BehaviourPost());

            SetupAuto.Setup(player);
            SetupBasic.Setup(player);
            SetupCountdown.Setup(player);
            SetupGroup.Setup(player);
            SetupJump.Setup(player);
            SetupSand.Setup(player);
            SetupSequence.Setup(player);

            var entities = entityManager.Entities
                .SkipWhile(e => e != player)
                .ToList();
            entities.DoIf(e => !(e is EntityDraw), e => e.GoToFront());
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            var contentManager = Game1.instance.contentManager;
            if (contentManager.level == null)
            {
                return;
            }

            if (!SetupAuto.IsUsed
                && !SetupBasic.IsUsed
                && !SetupCountdown.IsUsed
                && !SetupGroup.IsUsed
                && !SetupJump.IsUsed
                && !SetupSand.IsUsed
                && !SetupSequence.IsUsed)
            {
                return;
            }

            ModSounds.Cleanup();

            SetupAuto.Cleanup();
            SetupBasic.Cleanup();
            SetupCountdown.Cleanup();
            SetupGroup.Cleanup();
            SetupJump.Cleanup();
            SetupSand.Cleanup();
            SetupSequence.Cleanup();
        }
    }
}
