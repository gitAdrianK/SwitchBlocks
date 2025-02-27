namespace SwitchBlocks.Setups
{
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;
    using SwitchBlocks.Settings;

    public static class SetupSand
    {
        public static void Setup(PlayerEntity player)
        {
            if (!SettingsSand.IsUsed)
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

            // XXX: Do not register the same behaviour for multiple blocks if the behaviour changes
            // velocity or position! This technically needs updating, but I have to consider
            // Ghost of the Immortal Babe breaking!
            var behaviourSandPlatform = new BehaviourSandPlatform();
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSandPlatform);
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSandPlatform);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), new BehaviourSandLever());
        }

        public static void Cleanup()
        {
            if (!SettingsSand.IsUsed)
            {
                return;
            }

            DataSand.Instance.SaveToFile();
            DataSand.Instance.Reset();

            SettingsSand.IsUsed = false;
        }
    }
}
