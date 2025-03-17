namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;

    public static class SetupCountdown
    {
        /// <summary>
        /// Whether the countdown block appears inside the hitbox file and counts as used.
        /// </summary>
        public static bool IsUsed { get; set; } = false;

        /// <summary>
        /// Screens that contain a wind enable block.
        /// </summary>
        public static HashSet<int> WindEnabled { get; set; } = new HashSet<int>();

        public static void Setup(PlayerEntity player)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataCountdown.Instance;

            var entityLogic = new EntityLogicCountdown();
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Platforms,
                FactoryDrawables.BlockType.Countdown,
                entityLogic);
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Levers,
                FactoryDrawables.BlockType.Countdown,
                entityLogic);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOn), new BehaviourCountdownOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOff), new BehaviourCountdownOff());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), new BehaviourCountdownLever());
        }

        public static void Cleanup()
        {
            if (!IsUsed)
            {
                return;
            }

            DataCountdown.Instance.SaveToFile();
            DataCountdown.Instance.Reset();

            IsUsed = false;
        }
    }
}
