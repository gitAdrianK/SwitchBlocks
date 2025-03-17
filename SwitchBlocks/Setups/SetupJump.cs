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

    public static class SetupJump
    {
        /// <summary>
        /// Whether the jump block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;

        /// <summary>
        /// Screens that contain a wind enable block.
        /// </summary>
        public static HashSet<int> WindEnabled { get; set; } = new HashSet<int>();

        private static EntityLogicJump entityLogic;

        public static void Setup(PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataJump.Instance;

            entityLogic = new EntityLogicJump();
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Platforms,
                FactoryDrawables.BlockType.Jump,
                entityLogic);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOn), new BehaviourJumpOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOff), new BehaviourJumpOff());

            if (SettingsJump.ForceSwitch)
            {
                PlayerEntity.OnJumpCall += JumpSwitchUnsafe;
            }
            else
            {
                PlayerEntity.OnJumpCall += JumpSwitchSafe;
            }
        }

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

        private static void JumpSwitchUnsafe()
        {
            if (entityLogic != null && entityLogic.IsActiveOnCurrentScreen)
            {
                ModSounds.JumpFlip?.PlayOneShot();
            }
            DataJump.Instance.State = !DataJump.Instance.State;
        }

        private static void JumpSwitchSafe() => DataJump.Instance.SwitchOnceSafe = true;
    }
}
