namespace SwitchBlocks.Setups
{
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Factories;
    using SwitchBlocks.Settings;

    public static class SetupCountdown
    {
        public static void Setup(PlayerEntity player)
        {
            if (!SettingsCountdown.IsUsed)
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
            if (!SettingsCountdown.IsUsed)
            {
                return;
            }

            DataCountdown.Instance.SaveToFile();
            DataCountdown.Instance.Reset();

            SettingsCountdown.IsUsed = false;
        }
    }
}
