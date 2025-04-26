namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;

    /// <summary>
    /// Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupAuto
    {
        /// <summary>Whether the auto block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; } = false;
        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; set; } = new HashSet<int>();

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

            _ = DataAuto.Instance;

            var entityLogic = new EntityLogicAuto();
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Platforms,
                FactoryDrawables.BlockType.Auto,
                entityLogic);

            var body = player.m_body;
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoOn), new BehaviourAutoOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoOff), new BehaviourAutoOff());
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoReset), new BehaviourAutoReset());
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

            DataAuto.Instance.SaveToFile();
            DataAuto.Instance.Reset();

            IsUsed = false;
        }
    }
}
