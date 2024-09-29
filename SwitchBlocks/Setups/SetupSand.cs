using EntityComponent;
using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Entities;

namespace SwitchBlocks.Setups
{
    public static class SetupSand
    {
        public static void DoSetup(PlayerEntity player)
        {
            if (!ModBlocks.IsSandUsed)
            {
                return;
            }
            _ = EntitySandPlatforms.Instance;
            _ = EntitySandLevers.Instance;

            // XXX: Do not register the same behaviour for multiple blocks if the behaviour changes
            // velocity or position! This technically needs updating, but I have to consider
            // Ghost of the Immortal Babe breaking!
            BehaviourSandPlatform behaviourSandPlatform = new BehaviourSandPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSandPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSandPlatform);

            BehaviourSandLever behaviourSandLever = new BehaviourSandLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLever), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOn), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverOff), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolid), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOn), behaviourSandLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockSandLeverSolidOff), behaviourSandLever);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!ModBlocks.IsSandUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntitySandPlatforms.Instance);
            entityManager.RemoveObject(EntitySandLevers.Instance);
            EntitySandPlatforms.Instance.Reset();
            EntitySandLevers.Instance.Reset();
        }
    }
}