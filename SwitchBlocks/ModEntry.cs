using EntityComponent;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using SwitchBlocks.Factories;
using SwitchBlocks.Setups;
using System.IO;

namespace SwitchBlocks
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
            LevelManager.RegisterBlockFactory(new FactoryGroup());
            LevelManager.RegisterBlockFactory(new FactoryJump());
            LevelManager.RegisterBlockFactory(new FactorySand());
            LevelManager.RegisterBlockFactory(new FactorySequence());

            Harmony harmony = new Harmony(ModStrings.HARMONY);
            new Patching.BodyComp(harmony);
            new Patching.EndingManager(harmony);
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            if (!IsModUsed())
            {
                return;
            }

            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            ModSettings.Load();

            ModSounds.Load();

            SetupAuto.DoSetup(player);
            SetupBasic.DoSetup(player);
            SetupCountdown.DoSetup(player);
            SetupGroup.DoSetup(player);
            SetupJump.DoSetup(player);
            SetupSand.DoSetup(player);
            SetupSequence.DoSetup(player);

            EntityManager.instance.MoveToFront(player);
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            if (!IsModUsed())
            {
                return;
            }

            EntityManager entityManager = EntityManager.instance;

            ModSounds.Cleanup();

            SetupAuto.DoCleanup(entityManager);
            SetupBasic.DoCleanup(entityManager);
            SetupCountdown.DoCleanup(entityManager);
            SetupGroup.DoCleanup(entityManager);
            SetupJump.DoCleanup(entityManager);
            SetupSand.DoCleanup(entityManager);
            SetupSequence.DoCleanup(entityManager);
        }

        /// <summary>
        /// Determines if the stuff used for the mod is present and thus the mod is used.
        /// </summary>
        /// <returns>true if the mods supposed to be loaded, false otherwise</returns>
        private static bool IsModUsed()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}";

            // Level being null means the vanilla map is being started/ended.
            // And the vanilla map has no switch blocks.
            if (contentManager.level == null)
            {
                return false;
            }
            // If theres no switchBlocksMod folder the mod isn't used.
            if (!Directory.Exists(path))
            {
                return false;
            }
            return true;
        }
    }
}
