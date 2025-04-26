namespace SwitchBlocks.Setups
{
    using JumpKing.API;
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
    public static class SetupSand
    {
        /// <summary>Whether the sand block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; } = false;

        /// <summary>
        /// Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="player">Player to register block behaviours to.</param>
        public static void Setup(PlayerEntity player, ICollisionQuery collisionQuery)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataSand.Instance;

            var entityLogic = new EntityLogicSand();
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Platforms,
                FactoryDrawables.BlockType.Sand,
                entityLogic);
            FactoryDrawables.CreateDrawables(
                FactoryDrawables.DrawType.Levers,
                FactoryDrawables.BlockType.Sand,
                entityLogic);

            var body = player.m_body;
            if (SettingsSand.IsV2)
            {
                // To keep legacy and GotIB without change the new behaviour is behind a v2 setting.
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOn), new BehaviourSandOn(collisionQuery));
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOff), new BehaviourSandOff(collisionQuery));
            }
            else
            {
                // XXX: Do not register the same behaviour for multiple blocks if the behaviour changes
                // velocity or position! This technically needs updating, but I have to consider
                // Ghost of the Immortal Babe breaking!
                var behaviourSandPlatform = new BehaviourSandLegacy();
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSandPlatform);
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSandPlatform);
            }

            _ = body.RegisterBlockBehaviour(typeof(BlockSandLever), new BehaviourSandLever());
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

            DataSand.Instance.SaveToFile();
            DataSand.Instance.Reset();

            IsUsed = false;
        }
    }
}
