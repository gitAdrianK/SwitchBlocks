namespace SwitchBlocks.Setups
{
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Settings;

    public static class SetupSand
    {
        private static EntitySandLevers entitySandLevers;
        private static EntitySandPlatforms entitySandPlatforms;

        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsSand.IsUsed)
            {
                return;
            }

            _ = DataSand.Instance;

            entitySandLevers = new EntitySandLevers();
            entitySandPlatforms = new EntitySandPlatforms();

            // XXX: Do not register the same behaviour for multiple blocks if the behaviour changes
            // velocity or position! This technically needs updating, but I have to consider
            // Ghost of the Immortal Babe breaking!
            var behaviourSandPlatform = new BehaviourSandPlatform();
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSandPlatform);
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSandPlatform);

            var behaviourSandLever = new BehaviourSandLever();
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOn), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOff), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolid), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOn), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOff), behaviourSandLever);
        }

        public static void DoCleanup()
        {
            if (!SettingsSand.IsUsed)
            {
                return;
            }

            entitySandLevers.Destroy();
            entitySandPlatforms.Destroy();
            entitySandLevers = null;
            entitySandPlatforms = null;

            DataSand.Instance.SaveToFile();
            DataSand.Instance.Reset();

            SettingsSand.IsUsed = false;
        }
    }
}
