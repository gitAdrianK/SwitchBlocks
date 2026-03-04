namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using System.IO;
    using Behaviours;
    using Blocks;
    using Data;
    using Entities;
    using Factories.Drawables;
    using JumpKing.Player;
    using Settings;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupBasic
    {
        /// <summary>Whether the basic block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; } = new HashSet<int>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// ///
        /// <param name="settings">Settings of the basic type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        public static void Setup(SettingsBasic settings, BodyComp body)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataBasic.Instance;

            var entityLogic = new EntityLogicBasic(settings);

            var xmlPath = Path.Combine(ModEntry.ModPath, ModConstants.Basic);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, ModEntry.TexturePath, DataBasic.Instance);
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataBasic.Instance, entityLogic);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataBasic.Instance,
                    entityLogic, false);
            }
            else
            {
                FactoryDrawables.CreateDrawablesLegacy(
                    FactoryDrawables.DrawType.Platforms,
                    FactoryDrawables.BlockType.Basic,
                    entityLogic);
                FactoryDrawables.CreateDrawablesLegacy(
                    FactoryDrawables.DrawType.Levers,
                    FactoryDrawables.BlockType.Basic,
                    entityLogic);
                FactoryDrawables.CreateDrawablesLegacy(
                    FactoryDrawables.DrawType.Conveyors,
                    FactoryDrawables.BlockType.Basic,
                    entityLogic);
            }

            _ = body.RegisterBlockBehaviour(typeof(BlockBasicOn), new BehaviourBasicOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockBasicOff), new BehaviourBasicOff());
            _ = body.RegisterBlockBehaviour(typeof(BlockBasicLever), new BehaviourBasicLever(settings.LeverDirections));
        }

        /// <summary>
        ///     Cleans up saving data, resetting fields and does other required actions.
        /// </summary>
        public static void Cleanup()
        {
            if (!IsUsed)
            {
                return;
            }

            DataBasic.Instance.SaveToFile();
            DataBasic.Reset();

            IsUsed = false;
        }
    }
}
