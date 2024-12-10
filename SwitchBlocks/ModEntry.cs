using EntityComponent;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Factories;
using SwitchBlocks.Setups;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SwitchBlocks
{
    [JumpKingMod(ModStrings.MODNAME)]
    public static class ModEntry
    {
        public static List<Task> tasks = new List<Task>();

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

            // These behaviours are used as a way to create pre and post behaviour points
            // Mainly used to unify snow and ice behaviour, esp. ice behaviour since we don't
            // want to run the sliding function multiple times.
            player.m_body.RegisterBlockBehaviour<BlockPre>(new BehaviourPre());
            player.m_body.RegisterBlockBehaviour<BlockPost>(new BehaviourPost());

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

            tasks.ForEach(task => { task.Wait(); });
            tasks.Clear();
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
            // If there is no blocks.xml, no block can be activated.
            path = $"{path}blocks.xml";
            if (!File.Exists(path))
            {
                return false;
            }
            return true;
        }
    }
}
