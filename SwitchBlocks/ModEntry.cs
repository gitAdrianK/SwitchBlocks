using EntityComponent;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using SwitchBlocks.Factories;
using SwitchBlocks.Setups;
using System.IO;
using System.Threading.Tasks;

namespace SwitchBlocks
{
    [JumpKingMod(ModStrings.MODNAME)]
    public static class ModEntry
    {
        public static object threadLock = new object();

        private static Harmony harmony;

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

            harmony = new Harmony(ModStrings.HARMONY);
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

            Task sound = Task.Run(() => ModSounds.Load());

            Task auto = Task.Run(() => SetupAuto.DoSetup(player));
            Task basic = Task.Run(() => SetupBasic.DoSetup(player));
            Task countdown = Task.Run(() => SetupCountdown.DoSetup(player));
            Task group = Task.Run(() => SetupGroup.DoSetup(player));
            Task jump = Task.Run(() => SetupJump.DoSetup(player));
            Task sand = Task.Run(() => SetupSand.DoSetup(player));
            Task sequence = Task.Run(() => SetupSequence.DoSetup(player));

            Task.WaitAll(
                auto,
                basic,
                countdown,
                group,
                jump,
                sand,
                sequence,
                sound);

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

            Task sound = Task.Run(() => ModSounds.Cleanup());

            Task auto = Task.Run(() => SetupAuto.DoCleanup(entityManager));
            Task basic = Task.Run(() => SetupBasic.DoCleanup(entityManager));
            Task countdown = Task.Run(() => SetupCountdown.DoCleanup(entityManager));
            Task group = Task.Run(() => SetupGroup.DoCleanup(entityManager));
            Task jump = Task.Run(() => SetupJump.DoCleanup(entityManager));
            Task sand = Task.Run(() => SetupSand.DoCleanup(entityManager));
            Task sequence = Task.Run(() => SetupSequence.DoCleanup(entityManager));

            Task.WaitAll(
                auto,
                basic,
                countdown,
                group,
                jump,
                sand,
                sequence,
                sound);
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
            if (contentManager.level == null)
            {
                return false;
            }
            if (!Directory.Exists(path))
            {
                return false;
            }
            return true;
        }
    }
}
