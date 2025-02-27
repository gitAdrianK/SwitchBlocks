namespace SwitchBlocks.Setups
{
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;
    using SwitchBlocks.Settings;

    public static class SetupAuto
    {
        public static void Setup(PlayerEntity player)
        {
            if (!SettingsAuto.IsUsed)
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
            if (!SettingsAuto.IsUsed)
            {
                return;
            }

            DataAuto.Instance.SaveToFile();
            DataAuto.Instance.Reset();

            SettingsAuto.IsUsed = false;
        }
    }
}
