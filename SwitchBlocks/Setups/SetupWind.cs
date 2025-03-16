namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using JumpKing.Player;
    using SwitchBlocks.Behaviours;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;

    public class SetupWind
    {
        public static HashSet<int> WindEnabled { get; set; } = new HashSet<int>();

        public static void Setup(PlayerEntity player)
        {
            if (!SettingsWind.IsUsed)
            {
                return;
            }

            _ = DataWind.Instance;

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockWindLever), new BehaviourWindLever());
        }

        public static void Cleanup()
        {
            if (!SettingsWind.IsUsed)
            {
                return;
            }

            DataWind.Instance.SaveToFile();
            DataWind.Instance.Reset();

            SettingsWind.IsUsed = false;
        }
    }
}
