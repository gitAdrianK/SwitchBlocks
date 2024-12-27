namespace SwitchBlocks.Setups
{
    using EntityComponent;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Settings;

    public static class SetupCountdown
    {
        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsCountdown.IsUsed)
            {
                return;
            }

            _ = DataCountdown.Instance;

            _ = EntityCountdownPlatforms.Instance;
            _ = EntityCountdownLevers.Instance;

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOn), new BehaviourCountdownOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOff), new BehaviourCountdownOff());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), new BehaviourCountdownLever());
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsCountdown.IsUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntityCountdownPlatforms.Instance);
            entityManager.RemoveObject(EntityCountdownLevers.Instance);
            EntityCountdownPlatforms.Instance.Reset();
            EntityCountdownLevers.Instance.Reset();

            DataCountdown.Instance.SaveToFile();
            DataCountdown.Instance.Reset();

            SettingsCountdown.IsUsed = false;
        }
    }
}
