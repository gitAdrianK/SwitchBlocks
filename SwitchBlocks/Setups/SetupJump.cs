namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupJump
    {
        /// <summary>Whether the jump block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; set; } = new HashSet<int>();
        /// <summary>Logic entity of the jump blck type.</summary>
        private static EntityLogicJump EntityLogic { get; set; }

        /// <summary>
        /// Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="player">Player to register block behaviours to.</param>
        public static void Setup(PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataJump.Instance;

            EntityLogic = new EntityLogicJump();
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Platforms,
                FactoryDrawables.BlockType.Jump,
                EntityLogic);

            var body = player.m_body;
            _ = body.RegisterBlockBehaviour(typeof(BlockJumpOn), new BehaviourJumpOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockJumpOff), new BehaviourJumpOff());

            if (SettingsJump.ForceSwitch)
            {
                PlayerEntity.OnJumpCall += JumpSwitchUnsafe;
            }
            else
            {
                PlayerEntity.OnJumpCall += JumpSwitchSafe;
            }
        }

        /// <summary>
        /// Cleans up saving data, resetting fields and does other required actions.
        /// </summary>
        public static void Cleanup()
        {
            if (!IsUsed)
            {
                return;
            }

            if (SettingsJump.ForceSwitch)
            {
                PlayerEntity.OnJumpCall -= JumpSwitchUnsafe;
            }
            else
            {
                PlayerEntity.OnJumpCall -= JumpSwitchSafe;
            }

            DataJump.Instance.SaveToFile();
            DataJump.Instance.Reset();

            IsUsed = false;
        }

        /// <summary>Function to add to the OnJumpCall switching the state unsafely.</summary>
        private static void JumpSwitchUnsafe()
        {
            if (EntityLogic != null && EntityLogic.IsActiveOnCurrentScreen)
            {
                ModSounds.JumpFlip?.PlayOneShot();
            }
            DataJump.Instance.State = !DataJump.Instance.State;
        }

        /// <summary>Function to add to the OnJumpCall switching the state safely.</summary>
        private static void JumpSwitchSafe() => DataJump.Instance.SwitchOnceSafe = true;
    }
}
