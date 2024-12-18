using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Setups
{
    public static class SetupBasic
    {
        private static EntityBasicLevers entityBasicLevers;
        private static EntityBasicPlatforms entityBasicPlatforms;

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsBasic.IsUsed)
            {
                return;
            }

            _ = DataBasic.Instance;

            entityBasicLevers = new EntityBasicLevers();
            entityBasicPlatforms = new EntityBasicPlatforms();

            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOn), new BehaviourBasicOn());
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicOff), new BehaviourBasicOff());
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), new BehaviourBasicLever());
        }

        public static void DoCleanup()
        {
            if (!SettingsBasic.IsUsed)
            {
                return;
            }

            entityBasicLevers.Destroy();
            entityBasicPlatforms.Destroy();
            entityBasicLevers = null;
            entityBasicPlatforms = null;

            DataBasic.Instance.SaveToFile();
            DataBasic.Instance.Reset();

            SettingsBasic.IsUsed = false;
        }
    }
}
