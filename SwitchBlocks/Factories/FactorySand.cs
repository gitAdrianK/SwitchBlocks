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

    /// <summary>
    /// Factory for sand blocks.
    /// </summary>
    public class FactorySand : IBlockFactory
    {
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            ModBlocks.SAND_ON,
            ModBlocks.SAND_OFF,
            ModBlocks.SAND_LEVER,
            ModBlocks.SAND_LEVER_ON,
            ModBlocks.SAND_LEVER_OFF,
            ModBlocks.SAND_LEVER_SOLID,
            ModBlocks.SAND_LEVER_SOLID_ON,
            ModBlocks.SAND_LEVER_SOLID_OFF,
        };

        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.SAND_ON:
                case var _ when blockCode == ModBlocks.SAND_OFF:
                case var _ when blockCode == ModBlocks.SAND_LEVER_SOLID:
                case var _ when blockCode == ModBlocks.SAND_LEVER_SOLID_ON:
                case var _ when blockCode == ModBlocks.SAND_LEVER_SOLID_OFF:
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
                LastUsedMapId = level.ID;
            }
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.SAND_ON:
                    return new BlockSandOn(blockRect);
                case var _ when blockCode == ModBlocks.SAND_OFF:
                    return new BlockSandOff(blockRect);
                case var _ when blockCode == ModBlocks.SAND_LEVER:
                    return new BlockSandLever(blockRect);
                case var _ when blockCode == ModBlocks.SAND_LEVER_ON:
                    return new BlockSandLeverOn(blockRect);
                case var _ when blockCode == ModBlocks.SAND_LEVER_OFF:
                    return new BlockSandLeverOff(blockRect);
                case var _ when blockCode == ModBlocks.SAND_LEVER_SOLID:
                    return new BlockSandLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.SAND_LEVER_SOLID_ON:
                    return new BlockSandLeverSolidOn(blockRect);
                case var _ when blockCode == ModBlocks.SAND_LEVER_SOLID_OFF:
                    return new BlockSandLeverSolidOff(blockRect);
                default:
                    throw new InvalidOperationException($"{nameof(FactorySand)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
