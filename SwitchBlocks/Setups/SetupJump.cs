using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Setups
{
    public static class SetupJump
    {
        private static EntityJumpPlatforms entityJumpPlatforms;

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsJump.IsUsed)
            {
                return;
            }

            _ = DataJump.Instance;

            entityJumpPlatforms = new EntityJumpPlatforms();

            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOn), new BehaviourJumpOn());
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOff), new BehaviourJumpOff());

            PlayerEntity.OnJumpCall += JumpSwitch;
        }

        public static void DoCleanup()
        {
            if (!SettingsJump.IsUsed)
            {
                return;
            }

            entityJumpPlatforms.Destroy();
            entityJumpPlatforms = null;

            PlayerEntity.OnJumpCall -= JumpSwitch;

            DataJump.Instance.SaveToFile();
            DataJump.Instance.Reset();

            SettingsJump.IsUsed = false;
        }

        private static void JumpSwitch()
        {
            if (SettingsJump.ForceSwitch)
            {
                if (entityJumpPlatforms != null && entityJumpPlatforms.IsActiveOnCurrentScreen)
                {
                    ModSounds.JumpFlip?.PlayOneShot();
                }
                DataJump.State = !DataJump.State;
            }
            else
            {
                DataJump.SwitchOnceSafe = true;
            }
        }
    }
}
