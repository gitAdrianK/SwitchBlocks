namespace SwitchBlocks
{
    using System.Linq;
    using System.Reflection;
    using Behaviours;
    using Blocks;
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

            var harmony = new Harmony(ModConstants.Harmony);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        ///     Called by Jump King when the level starts.
        /// </summary>
        [OnLevelStart]
        [UsedImplicitly]
        public static void OnLevelStart()
        {
            var contentManager = Game1.instance.contentManager;
            if (contentManager.level is null)
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
            if (!IsUsed())
            {
                return;
            }

            var entityManager = EntityManager.instance;
            var player = entityManager.Find<PlayerEntity>();
            if (player is null)
            {
                return;
            }

            ModSettings.Setup();

            ModSounds.Setup(levelID);

            // These behaviours are used as a way to create pre- and post-behaviour points
            // Mainly used to unify snow and ice behaviour, esp. ice behaviour since we don't
            // want to run the sliding function multiple times.
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockPre), new BehaviourPre());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockPost), new BehaviourPost());

            SetupAuto.Setup(player);
            SetupBasic.Setup(player);
            SetupCountdown.Setup(player);
            SetupGroup.Setup(player);
            SetupJump.Setup(player);
            SetupSand.Setup(player, LevelManager.Instance);
            SetupSequence.Setup(player);

            // DoIf is a Harmony extension (that also does extra, for us unneeded, checks).
            var entities = entityManager.Entities
                .SkipWhile(entity => entity != player)
                .ToList();
            entities.ForEach(entity =>
            {
                if (!(entity is EntityDraw entityDraw))
                {
                    entity.GoToFront();
                }
                else
                {
                    if (entityDraw.IsForeground)
                    {
                        entityDraw.GoToFront();
                    }
                }
            });
        }

        /// <summary>
        ///     Called by Jump King when the level ends.
        /// </summary>
        [OnLevelEnd]
        [UsedImplicitly]
        public static void OnLevelEnd()
        {
            var contentManager = Game1.instance.contentManager;
            if (contentManager.level is null || !IsUsed())
            {
                return;
            }

            ModSettings.Cleanup();

            ModSounds.Cleanup();

            // IsUsed is false after Setup Cleanup.
            SetupSequence.Cleanup();
            SetupSand.Cleanup();
            SetupJump.Cleanup();
            SetupGroup.Cleanup();
            SetupCountdown.Cleanup();
            SetupBasic.Cleanup();
            SetupAuto.Cleanup();
        }

        /// <summary>
        ///     Checks if any of the blocks are used and loading settings, loading saves, registering behaviours etc.
        ///     can continue or if the mod should not insert itself into the current level played.
        ///     The block types themselves check if they are used too.
        /// </summary>
        /// <returns><c>true</c> if any block type is used, <c>false</c> otherwise.</returns>
        private static bool IsUsed() => SetupAuto.IsUsed
                                        || SetupBasic.IsUsed
                                        || SetupCountdown.IsUsed
                                        || SetupGroup.IsUsed
                                        || SetupJump.IsUsed
                                        || SetupSand.IsUsed
                                        || SetupSequence.IsUsed;
    }
}
