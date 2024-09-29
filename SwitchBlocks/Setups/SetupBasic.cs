﻿using EntityComponent;
using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Entities;

namespace SwitchBlocks.Setups
{
    public static class SetupBasic
    {
        public static void DoSetup(PlayerEntity player)
        {
            if (!ModBlocks.IsBasicUsed)
            {
                return;
            }
            _ = EntityBasicPlatforms.Instance;
            _ = EntityBasicLevers.Instance;

            BehaviourBasicIceOn behaviourBasicIceOn = new BehaviourBasicIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicIceOn), behaviourBasicIceOn);
            BehaviourBasicIceOff behaviourBasicIceOff = new BehaviourBasicIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicIceOff), behaviourBasicIceOff);

            BehaviourBasicSnow behaviourBasicSnow = new BehaviourBasicSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicSnowOn), behaviourBasicSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicSnowOff), behaviourBasicSnow);

            BehaviourBasicLever behaviourBasicLever = new BehaviourBasicLever();
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLever), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverOff), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolid), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOn), behaviourBasicLever);
            player.m_body.RegisterBlockBehaviour(typeof(BlockBasicLeverSolidOff), behaviourBasicLever);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!ModBlocks.IsBasicUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntityBasicPlatforms.Instance);
            entityManager.RemoveObject(EntityBasicLevers.Instance);
            EntityBasicPlatforms.Instance.Reset();
            EntityBasicLevers.Instance.Reset();
        }
    }
}