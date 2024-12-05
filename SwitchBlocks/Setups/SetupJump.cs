using EntityComponent;
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
        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsJump.IsUsed)
            {
                return;
            }

            _ = DataJump.Instance;

            _ = EntityJumpPlatforms.Instance;

            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOn), new BehaviourJumpOn());
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOff), new BehaviourJumpOff());

            PlayerEntity.OnJumpCall += JumpSwitch;
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsJump.IsUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntityJumpPlatforms.Instance);
            EntityJumpPlatforms.Instance.Reset();

            PlayerEntity.OnJumpCall -= JumpSwitch;

            DataJump.Instance.SaveToFile();
            DataJump.Instance.Reset();

            SettingsJump.IsUsed = false;
        }

        private static void JumpSwitch()
        {
            if (SettingsJump.ForceSwitch)
            {
                if (EntityJumpPlatforms.Instance.IsActiveOnCurrentScreen)
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
