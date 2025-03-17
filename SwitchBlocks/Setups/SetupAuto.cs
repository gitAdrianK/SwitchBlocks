namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;

    public static class SetupAuto
    {
        /// <summary>
        /// Whether the auto block appears inside the hitbox file and counts as used.
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

            _ = DataAuto.Instance;

            var entityLogic = new EntityLogicAuto();
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Platforms,
                FactoryDrawables.BlockType.Auto,
                entityLogic);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOn), new BehaviourAutoOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOff), new BehaviourAutoOff());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockAutoReset), new BehaviourAutoReset());
        }

        public static void Cleanup()
        {
            if (!IsUsed)
            {
                return;
            }

            DataAuto.Instance.SaveToFile();
            DataAuto.Instance.Reset();

            IsUsed = false;
        }
    }
}
