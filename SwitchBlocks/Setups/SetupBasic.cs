namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;

    public static class SetupBasic
    {
        /// <summary>
        /// Whether the basic block appears inside the hitbox file and counts as used.
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

            _ = DataBasic.Instance;

            var entityLogic = new EntityLogicBasic();
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Platforms,
                FactoryDrawables.BlockType.Basic,
                entityLogic);
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Levers,
                FactoryDrawables.BlockType.Basic,
                entityLogic);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOn), new BehaviourBasicOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOff), new BehaviourBasicOff());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), new BehaviourBasicLever());
        }

        public static void Cleanup()
        {
            if (!IsUsed)
            {
                return;
            }

            DataBasic.Instance.SaveToFile();
            DataBasic.Instance.Reset();

            IsUsed = false;
        }
    }
}
