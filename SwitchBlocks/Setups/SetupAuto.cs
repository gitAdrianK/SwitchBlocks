using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Setups
{
    public static class SetupAuto
    {
        private static EntityAutoPlatforms entityAutoPlatforms;

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsAuto.IsUsed)
            {
                return;
            }

            _ = DataAuto.Instance;

            entityAutoPlatforms = new EntityAutoPlatforms();

            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOn), new BehaviourAutoOn());
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOff), new BehaviourAutoOff());
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoReset), new BehaviourAutoReset());
        }

        public static void DoCleanup()
        {
            if (!SettingsAuto.IsUsed)
            {
                return;
            }

            entityAutoPlatforms.Destroy();
            entityAutoPlatforms = null;

            DataAuto.Instance.SaveToFile();
            DataAuto.Instance.Reset();

            SettingsAuto.IsUsed = false;
        }
    }
}
