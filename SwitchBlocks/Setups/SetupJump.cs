using EntityComponent;
using JumpKing;
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

            lock (player)
            {
                BehaviourJumpIceOn behaviourJumpIceOn = new BehaviourJumpIceOn();
                player.m_body.RegisterBlockBehaviour(typeof(BlockJumpIceOn), behaviourJumpIceOn);
                BehaviourJumpIceOff behaviourJumpIceOff = new BehaviourJumpIceOff();
                player.m_body.RegisterBlockBehaviour(typeof(BlockJumpIceOff), behaviourJumpIceOff);

                BehaviourJumpSnow behaviourJumpSnow = new BehaviourJumpSnow();
                player.m_body.RegisterBlockBehaviour(typeof(BlockJumpSnowOn), behaviourJumpSnow);
                player.m_body.RegisterBlockBehaviour(typeof(BlockJumpSnowOff), behaviourJumpSnow);
            }

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
        }

        private static void JumpSwitch()
        {
            if (EntityJumpPlatforms.Instance.PlatformDictionary != null
                && EntityJumpPlatforms.Instance.PlatformDictionary.ContainsKey(Camera.CurrentScreen))
            {
                ModSounds.JumpFlip?.PlayOneShot();
            }
            DataJump.State = !DataJump.State;
        }
    }
}
