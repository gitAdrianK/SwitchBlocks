namespace SwitchBlocks.Setups
{
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;
    using SwitchBlocks.Settings;

    public static class SetupBasic
    {
        public static void Setup(PlayerEntity player)
        {
            if (!SettingsBasic.IsUsed)
            {
                return;
            }

            _ = DataBasic.Instance;

            _ = new EntityLogicBasic();

            FactoryDrawables.CreateDrawables(FactoryDrawables.DrawType.Platforms, FactoryDrawables.BlockType.Basic);
            FactoryDrawables.CreateDrawables(FactoryDrawables.DrawType.Levers, FactoryDrawables.BlockType.Basic);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOn), new BehaviourBasicOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOff), new BehaviourBasicOff());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), new BehaviourBasicLever());
        }

        public static void Cleanup()
        {
            if (!SettingsBasic.IsUsed)
            {
                return;
            }

            DataBasic.Instance.SaveToFile();
            DataBasic.Instance.Reset();

            SettingsBasic.IsUsed = false;
        }
    }
}
