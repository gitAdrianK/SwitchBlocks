namespace SwitchBlocks.Factories
{
    using System;
    using System.Collections.Generic;
    using JumpKing.API;
    using JumpKing.Level;
    using JumpKing.Level.Sampler;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Setups;

    /// <summary>
    /// Factory for wind blocks.
    /// </summary>
    public class FactoryWind : IBlockFactory
    {
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            ModBlocks.WIND_ENABLE,
            ModBlocks.WIND_LEVER,
            ModBlocks.WIND_LEVER_ON,
            ModBlocks.WIND_LEVER_OFF,
            ModBlocks.WIND_LEVER_SOLID,
            ModBlocks.WIND_LEVER_SOLID_ON,
            ModBlocks.WIND_LEVER_SOLID_OFF,
        };

        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.WIND_LEVER_SOLID:
                case var _ when blockCode == ModBlocks.WIND_LEVER_SOLID_ON:
                case var _ when blockCode == ModBlocks.WIND_LEVER_SOLID_OFF:
                    return true;
                default:
                    break;
            }
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                SetupWind.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.WIND_ENABLE:
                    _ = SetupWind.WindEnabled.Add(currentScreen);
                    return new BlockWindEnable(blockRect);
                case var _ when blockCode == ModBlocks.WIND_LEVER:
                    return new BlockWindLever(blockRect);
                case var _ when blockCode == ModBlocks.WIND_LEVER_ON:
                    return new BlockWindLeverOn(blockRect);
                case var _ when blockCode == ModBlocks.WIND_LEVER_OFF:
                    return new BlockWindLeverOff(blockRect);
                case var _ when blockCode == ModBlocks.WIND_LEVER_SOLID:
                    return new BlockWindLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.WIND_LEVER_SOLID_ON:
                    return new BlockWindLeverSolidOn(blockRect);
                case var _ when blockCode == ModBlocks.WIND_LEVER_SOLID_OFF:
                    return new BlockWindLeverSolidOff(blockRect);
                default:
                    throw new InvalidOperationException($"{nameof(FactoryWind)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
