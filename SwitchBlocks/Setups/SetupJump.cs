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

            BehaviourJumpPlatform behaviourJumpPlatform = new BehaviourJumpPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOn), behaviourJumpPlatform);

            BehaviourJumpIceOn behaviourJumpIceOn = new BehaviourJumpIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpIceOn), behaviourJumpIceOn);
            BehaviourJumpIceOff behaviourJumpIceOff = new BehaviourJumpIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpIceOff), behaviourJumpIceOff);

            BehaviourJumpSnow behaviourJumpSnow = new BehaviourJumpSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpSnowOn), behaviourJumpSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockJumpSnowOff), behaviourJumpSnow);

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
