namespace SwitchBlocks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Behaviours.Dummy;
    using Blocks.Dummy;
    using Controls;
    using Entities;
    using EntityComponent;
    using Factories;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing;
    using JumpKing.Level;
    using JumpKing.Mods;
    using JumpKing.Player;
    using Setups;
#if DEBUG
    using System.Diagnostics;
#endif

    [JumpKingMod(ModConstants.Modname)]
    public static class ModEntry
    {
        public static string RootModFolder { get; private set; }
        public static string TexturePath { get; private set; }
        public static Preferences Preferences { get; private set; }

        /// <summary>
        ///     Called by Jump King before the level loads.
        ///     -> OnGameStart
        /// </summary>
        [BeforeLevelLoad]
        [UsedImplicitly]
        public static void BeforeLevelLoad()
        {
#if DEBUG
            _ = Debugger.Launch();
#endif
            _ = LevelManager.RegisterBlockFactory(new FactoryAuto());
            _ = LevelManager.RegisterBlockFactory(new FactoryBasic());
            _ = LevelManager.RegisterBlockFactory(new FactoryCountdown());
            _ = LevelManager.RegisterBlockFactory(new FactoryGroup());
            _ = LevelManager.RegisterBlockFactory(new FactoryJump());
            _ = LevelManager.RegisterBlockFactory(new FactorySand());
            _ = LevelManager.RegisterBlockFactory(new FactorySequence());
            _ = LevelManager.RegisterBlockFactory(new FactoryThreshold());

            var assembly = Assembly.GetExecutingAssembly();

            new Harmony(ModConstants.Harmony).PatchAll(assembly);

            // Using the XmlSerializer for this because it happens on game start, so there's little need for it to be fast.
            // Unlike everything else that happens when the level loads,
            // specifically when the player is already visible/laying on the ground.
            var assemblyPath = Path.GetDirectoryName(assembly.Location) ?? throw new InvalidOperationException();
            try
            {
                var prefs = XmlSerializerHelper.Deserialize<Preferences>(Path.Combine(assemblyPath,
                    ModConstants.Settings));
                if (prefs.KeyBindings.Count != Enum.GetValues(typeof(EBinding)).Length)
                {
                    throw new Exception("Missing keybinding!");
                }

                Preferences = prefs;
            }
            catch
            {
                Preferences = new Preferences();
                XmlSerializerHelper.Serialize(Path.Combine(assemblyPath, ModConstants.Settings), Preferences);
            }

            Preferences.PropertyChanged += SaveSettingsOnFile;
        }

        /// <summary>
        ///     Called by Jump King when the level starts.
        /// </summary>
        [OnLevelStart]
        [UsedImplicitly]
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
            SetupThreshold.IsUsed = levelID == FactoryThreshold.LastUsedMapId;
            if (!IsUsed())
            {
                return;
            }

            var entityManager = EntityManager.instance;
            var player = entityManager.Find<PlayerEntity>();
            if (player == null)
            {
                return;
            }

            RootModFolder = Path.Combine(contentManager.root, ModConstants.Folder);
            TexturePath = Path.Combine(RootModFolder, ModConstants.Textures);

            var body = player.m_body;

            ModSounds.Setup(levelID);

            // These behaviours are used as a way to create pre- and post-behaviour points as well as unify certain
            // behaviours into one. These are not player behaviours so we can use priorities as well as cheese
            // the "Player behaviour modifiers detected" message.
            var behaviourPre = new BehaviourPre();
            _ = body.RegisterBlockBehaviour(typeof(BlockPre), behaviourPre);
            _ = body.RegisterBlockBehaviour(typeof(BlockConveyor), new BehaviourConveyor());
            _ = body.RegisterBlockBehaviour(typeof(BlockPost), new BehaviourPost());

            var foregroundEntities = new List<EntityDraw>();
            var midgroundEntities = new List<EntityDraw>();

            var settings = new ModSettings();

            SetupAuto.Setup(behaviourPre, settings.SettingsAuto, body, foregroundEntities, midgroundEntities);
            SetupBasic.Setup(behaviourPre, settings.SettingsBasic, body, foregroundEntities, midgroundEntities);
            SetupCountdown.Setup(behaviourPre, settings.SettingsCountdown, body, foregroundEntities, midgroundEntities);
            SetupGroup.Setup(settings.SettingsGroup, body, foregroundEntities, midgroundEntities);
            SetupJump.Setup(behaviourPre, settings.SettingsJump, player, foregroundEntities, midgroundEntities);
            SetupSand.Setup(settings.SettingsSand, body, LevelManager.Instance, foregroundEntities, midgroundEntities);
            SetupSequence.Setup(settings.SettingsSequence, body, foregroundEntities, midgroundEntities);
            SetupThreshold.Setup(settings.SettingsThreshold, body, foregroundEntities, midgroundEntities, levelID);

            var entities = entityManager.Entities.ToList();
            foreach (var entity in entities.Where(entity => !(entity is EntityDraw)))
            {
                if (entity is PlayerEntity)
                {
                    foreach (var midgroundEntity in midgroundEntities)
                    {
                        midgroundEntity.GoToFront();
                    }
                }

                entity.GoToFront();
            }

            foreach (var foregroundEntity in foregroundEntities)
            {
                foregroundEntity.GoToFront();
            }
        }

        /// <summary>
        ///     Called by Jump King when the level ends.
        /// </summary>
        [OnLevelEnd]
        [UsedImplicitly]
        public static void OnLevelEnd()
        {
            var contentManager = Game1.instance.contentManager;
            if (contentManager.level == null || !IsUsed())
            {
                return;
            }

            ModSounds.Cleanup();

            // IsUsed is false after Setup Cleanup.
            SetupAuto.Cleanup();
            SetupBasic.Cleanup();
            SetupCountdown.Cleanup();
            SetupGroup.Cleanup();
            SetupJump.Cleanup();
            SetupSand.Cleanup();
            SetupSequence.Cleanup();
            SetupThreshold.Cleanup();

            ModDebug.Reset();
        }

        /// <summary>
        ///     Checks if any of the blocks are used and loading settings, loading saves, registering behaviours etc.
        ///     can continue or if the mod should not insert itself into the current level played.
        ///     The block types themselves check if they are used too.
        /// </summary>
        /// <returns><c>true</c> if any block type is used, <c>false</c> otherwise.</returns>
        public static bool IsUsed() => SetupAuto.IsUsed
                                       || SetupBasic.IsUsed
                                       || SetupCountdown.IsUsed
                                       || SetupGroup.IsUsed
                                       || SetupJump.IsUsed
                                       || SetupSand.IsUsed
                                       || SetupSequence.IsUsed
                                       || SetupThreshold.IsUsed;

        /// <summary>
        ///     Save settings when a field changes.
        /// </summary>
        /// <param name="sender">Object sending the event.</param>
        /// <param name="args">Arguments passed by the event</param>
        /// <exception cref="InvalidOperationException">Thrown when getting the assembly location fails.</exception>
        private static void SaveSettingsOnFile(object sender, PropertyChangedEventArgs args)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                               throw new InvalidOperationException();
            XmlSerializerHelper.Serialize(Path.Combine(assemblyPath, ModConstants.Settings), Preferences);
        }
    }
}
