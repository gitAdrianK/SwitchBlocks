namespace SwitchBlocks.Setups
{
    using EntityComponent;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Settings;

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

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOn), new BehaviourJumpOn());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockJumpOff), new BehaviourJumpOff());

            if (SettingsJump.ForceSwitch)
            {
                PlayerEntity.OnJumpCall += JumpSwitchUnsafe;
            }
            else
            {
                PlayerEntity.OnJumpCall += JumpSwitchSafe;
            }
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsJump.IsUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntityJumpPlatforms.Instance);
            EntityJumpPlatforms.Instance.Reset();

            if (SettingsJump.ForceSwitch)
            {
                PlayerEntity.OnJumpCall -= JumpSwitchUnsafe;
            }
            else
            {
                PlayerEntity.OnJumpCall -= JumpSwitchSafe;
            }

            DataJump.Instance.SaveToFile();
            DataJump.Instance.Reset();

            SettingsJump.IsUsed = false;
        }

        private static void JumpSwitchUnsafe()
        {
            if (EntityJumpPlatforms.Instance.IsActiveOnCurrentScreen)
            {
                ModSounds.JumpFlip?.PlayOneShot();
            }
            DataJump.State = !DataJump.State;
        }

        private static void JumpSwitchSafe() => DataJump.SwitchOnceSafe = true;
    }
}
