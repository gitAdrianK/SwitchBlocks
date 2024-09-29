using EntityComponent;
using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Entities;

namespace SwitchBlocks.Setups
{
    public static class SetupCountdown
    {
        public static void DoSetup(PlayerEntity player)
        {
            if (!ModBlocks.IsCountdownUsed)
            {
                return;
            }
            _ = EntityCountdownPlatforms.Instance;
            _ = EntityCountdownLevers.Instance;

            BehaviourCountdownPlatform behaviourCountdownPlatform = new BehaviourCountdownPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOn), behaviourCountdownPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownOff), behaviourCountdownPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOn), behaviourCountdownPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOff), behaviourCountdownPlatform);

            BehaviourCountdownIceOn behaviourCountdownIceOn = new BehaviourCountdownIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOn), behaviourCountdownIceOn);
            BehaviourCountdownIceOff behaviourCountdownIceOff = new BehaviourCountdownIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownIceOff), behaviourCountdownIceOff);

            BehaviourCountdownSnow behaviourCountdownSnow = new BehaviourCountdownSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownSnowOn), behaviourCountdownSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownSnowOff), behaviourCountdownSnow);

            BehaviourCountdownLever behaviourCountdownLever = new BehaviourCountdownLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLever), behaviourCountdownLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockCountdownLeverSolid), behaviourCountdownLever);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!ModBlocks.IsCountdownUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntityCountdownPlatforms.Instance);
            entityManager.RemoveObject(EntityCountdownLevers.Instance);
            EntityCountdownPlatforms.Instance.Reset();
            EntityCountdownLevers.Instance.Reset();
        }
    }
}
