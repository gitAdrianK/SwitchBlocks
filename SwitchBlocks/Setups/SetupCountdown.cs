using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Setups
{
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

            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOn), new BehaviourCountdownOn());
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOff), new BehaviourCountdownOff());
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), new BehaviourCountdownLever());
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
