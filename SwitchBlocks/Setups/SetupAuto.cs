﻿using EntityComponent;
using JumpKing.Player;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Setups
{
    public static class SetupAuto
    {
        public static void DoSetup(PlayerEntity player)
        {
            if (!SettingsAuto.IsUsed)
            {
                return;
            }
            _ = DataAuto.Instance;

            _ = EntityAutoPlatforms.Instance;

            BehaviourAutoPlatform behaviourAutoPlatform = new BehaviourAutoPlatform();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOn), behaviourAutoPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoOff), behaviourAutoPlatform);

            BehaviourAutoIceOn behaviourAutoIceOn = new BehaviourAutoIceOn();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoIceOn), behaviourAutoIceOn);
            BehaviourAutoIceOff behaviourAutoIceOff = new BehaviourAutoIceOff();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoIceOff), behaviourAutoIceOff);

            BehaviourAutoSnow behaviourAutoSnow = new BehaviourAutoSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoSnowOn), behaviourAutoSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoSnowOff), behaviourAutoSnow);

            BehaviourAutoReset behaviourAutoReset = new BehaviourAutoReset();
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoReset), behaviourAutoReset);
            player.m_body.RegisterBlockBehaviour(typeof(BlockAutoResetFull), behaviourAutoReset);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!SettingsAuto.IsUsed)
            {
                return;
            }
            entityManager.RemoveObject(EntityAutoPlatforms.Instance);
            EntityAutoPlatforms.Instance.Reset();

            DataAuto.Instance.SaveToFile();
            DataAuto.Instance.Reset();
        }
    }
}