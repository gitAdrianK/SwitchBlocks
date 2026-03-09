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
    using Patches;
    using Settings;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupJump
    {
        /// <summary>Whether the jump block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; } = new HashSet<int>();

        /// <summary>Logic entity of the jump block type.</summary>
        public static EntityLogicJump EntityLogicJump { get; set; }

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="settings">Settings of the jump type.</param>
        /// <param name="player"><see cref="PlayerEntity" /> to register block behaviours to and pass to the logic.</param>
        public static void Setup(SettingsJump settings, PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataJump.Instance;

            EntityLogicJump = new EntityLogicJump(settings, player);

            var xmlPath = Path.Combine(ModEntry.RootModFolder, ModConstants.Jump);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataJump.Instance, EntityLogicJump);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataJump.Instance,
                    EntityLogicJump, false);
            }
            else
            {
                xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Jump);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataJump.Instance, EntityLogicJump);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Jump);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataJump.Instance, EntityLogicJump, false, true);
            }

            var body = player.m_body;
            _ = body.RegisterBlockBehaviour(typeof(BlockJumpOn), new BehaviourJumpOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockJumpOff), new BehaviourJumpOff());

            if (settings.ForceSwitch)
            {
                PlayerEntity.OnJumpCall += JumpSwitchUnsafe;
            }
            else
            {
                PlayerEntity.OnJumpCall += JumpSwitchSafe;
            }

            // ReSharper disable once InvertIf
            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicJump = EntityLogicJump;
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

            EntityLogicJump.Destroy();
            EntityLogicJump = null;

            PlayerEntity.OnJumpCall -= JumpSwitchUnsafe;
            PlayerEntity.OnJumpCall -= JumpSwitchSafe;

            DataJump.Instance.SaveToFile();
            DataJump.Reset();

            IsUsed = false;
        }

        /// <summary>Function to add to the OnJumpCall switching the state unsafely.</summary>
        public static void JumpSwitchUnsafe()
        {
            if (EntityLogicJump != null && EntityLogicJump.IsActiveOnCurrentScreen)
            {
                ModSounds.JumpFlip?.PlayOneShot();
            }

            DataJump.Instance.State = !DataJump.Instance.State;
            DataJump.Instance.Tick = PatchAchievementManager.GetTick();
        }

        /// <summary>Function to add to the OnJumpCall switching the state safely.</summary>
        public static void JumpSwitchSafe() => DataJump.Instance.SwitchOnceSafe = true;
    }
}
