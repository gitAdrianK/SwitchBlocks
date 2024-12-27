namespace SwitchBlocks.Setups
{
    using EntityComponent;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Settings;

    public static class SetupSand
    {
        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsSand.IsUsed)
            {
                return;
            }

            _ = DataSand.Instance;

            _ = EntitySandPlatforms.Instance;
            _ = EntitySandLevers.Instance;

            // XXX: Do not register the same behaviour for multiple blocks if the behaviour changes
            // velocity or position! This technically needs updating, but I have to consider
            // Ghost of the Immortal Babe breaking!
            var behaviourSandPlatform = new BehaviourSandPlatform();
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSandPlatform);
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSandPlatform);

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), new BehaviourSandLever());
            //var behaviourSandLever = new BehaviourSandLever();
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOn), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOff), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolid), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOn), behaviourSandLever);
            //player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOff), behaviourSandLever);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsSand.IsUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntitySandPlatforms.Instance);
            entityManager.RemoveObject(EntitySandLevers.Instance);
            EntitySandPlatforms.Instance.Reset();
            EntitySandLevers.Instance.Reset();

            DataSand.Instance.SaveToFile();
            DataSand.Instance.Reset();

            SettingsSand.IsUsed = false;
        }
    }
}
