namespace SwitchBlocks.Setups
{
    using EntityComponent;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Settings;

    public static class SetupBasic
    {
        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsBasic.IsUsed)
            {
                return;
            }

            _ = DataBasic.Instance;

            _ = EntityBasicPlatforms.Instance;
            _ = EntityBasicLevers.Instance;

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOn), new BehaviourBasicOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOff), new BehaviourBasicOff());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), new BehaviourBasicLever());
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsBasic.IsUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntityBasicPlatforms.Instance);
            entityManager.RemoveObject(EntityBasicLevers.Instance);
            EntityBasicPlatforms.Instance.Reset();
            EntityBasicLevers.Instance.Reset();

            DataBasic.Instance.SaveToFile();
            DataBasic.Instance.Reset();

            SettingsBasic.IsUsed = false;
        }
    }
}
