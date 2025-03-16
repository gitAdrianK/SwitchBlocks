namespace SwitchBlocks
{
    using System.Collections.Generic;
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
    using SwitchBlocks.Settings;
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
            //_ = Debugger.Launch();

            _ = LevelManager.RegisterBlockFactory(new FactoryAuto());
            _ = LevelManager.RegisterBlockFactory(new FactoryBasic());
            _ = LevelManager.RegisterBlockFactory(new FactoryCountdown());
            _ = LevelManager.RegisterBlockFactory(new FactoryGroup());
            _ = LevelManager.RegisterBlockFactory(new FactoryJump());
            _ = LevelManager.RegisterBlockFactory(new FactorySand());
            _ = LevelManager.RegisterBlockFactory(new FactorySequence());
            _ = LevelManager.RegisterBlockFactory(new FactoryWind());

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
            SettingsAuto.IsUsed = levelID == FactoryAuto.LastUsedMapId;
            SettingsBasic.IsUsed = levelID == FactoryBasic.LastUsedMapId;
            SettingsCountdown.IsUsed = levelID == FactoryCountdown.LastUsedMapId;
            SettingsGroup.IsUsed = levelID == FactoryGroup.LastUsedMapId;
            SettingsJump.IsUsed = levelID == FactoryJump.LastUsedMapId;
            SettingsSand.IsUsed = levelID == FactorySand.LastUsedMapId;
            SettingsSequence.IsUsed = levelID == FactorySequence.LastUsedMapId;
            SettingsWind.IsUsed = levelID == FactoryWind.LastUsedMapId;

            if (!SettingsAuto.IsUsed
                && !SettingsBasic.IsUsed
                && !SettingsCountdown.IsUsed
                && !SettingsGroup.IsUsed
                && !SettingsJump.IsUsed
                && !SettingsSand.IsUsed
                && !SettingsSequence.IsUsed
                && !SettingsWind.IsUsed)
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
            SetupWind.Setup(player);

            var entities = new List<Entity>();
            var playerFound = false;
            foreach (var entity in entityManager.Entities)
            {
                if (entity == player)
                {
                    playerFound = true;
                }
                if (playerFound && !(entity is EntityDraw))
                {
                    entities.Add(entity);
                }
            }
            foreach (var entity in entities)
            {
                entityManager.MoveToFront(entity);
            }
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

            if (!SettingsAuto.IsUsed
                && !SettingsBasic.IsUsed
                && !SettingsCountdown.IsUsed
                && !SettingsGroup.IsUsed
                && !SettingsJump.IsUsed
                && !SettingsSand.IsUsed
                && !SettingsSequence.IsUsed
                && !SettingsWind.IsUsed)
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
            SetupWind.Cleanup();
        }
    }
}
