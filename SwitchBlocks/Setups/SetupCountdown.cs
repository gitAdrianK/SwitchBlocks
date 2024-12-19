namespace SwitchBlocks.Setups
{
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Settings;

    public static class SetupCountdown
    {
        private static EntityCountdownLevers entityCountdownLevers;
        private static EntityCountdownPlatforms entityCountdownPlatforms;

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsCountdown.IsUsed)
            {
                return;
            }

            _ = DataCountdown.Instance;

            entityCountdownLevers = new EntityCountdownLevers();
            entityCountdownPlatforms = new EntityCountdownPlatforms();

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOn), new BehaviourCountdownOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOff), new BehaviourCountdownOff());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), new BehaviourCountdownLever());
        }

        public static void DoCleanup()
        {
            if (!SettingsCountdown.IsUsed)
            {
                return;
            }

            entityCountdownLevers.Destroy();
            entityCountdownPlatforms.Destroy();
            entityCountdownLevers = null;
            entityCountdownPlatforms = null;

            DataCountdown.Instance.SaveToFile();
            DataCountdown.Instance.Reset();

            SettingsCountdown.IsUsed = false;
        }
    }
}
