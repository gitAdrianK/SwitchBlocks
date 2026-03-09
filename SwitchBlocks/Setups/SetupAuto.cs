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
    public static class SetupAuto
    {
        /// <summary>Whether the auto block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; } = new HashSet<int>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="settings">Settings of the auto type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        public static void Setup(SettingsAuto settings, BodyComp body)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataAuto.Instance;

            var entityLogic = new EntityLogicAuto(settings);

            var xmlPath = Path.Combine(ModEntry.RootModFolder, ModConstants.Auto);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataAuto.Instance, entityLogic);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataAuto.Instance, entityLogic,
                    false);
            }
            else
            {
                xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Auto);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Auto);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic, false, true);
            }

            _ = body.RegisterBlockBehaviour(typeof(BlockAutoOn), new BehaviourAutoOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoOff), new BehaviourAutoOff());
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoReset), new BehaviourAutoReset(settings.DurationOff));

            // ReSharper disable once InvertIf
            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicAuto = entityLogic;
            }
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

            DataAuto.Instance.SaveToFile();
            DataAuto.Reset();

            IsUsed = false;
        }
    }
}
