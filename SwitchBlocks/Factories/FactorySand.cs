namespace SwitchBlocks.Factories
{
    using System;
    using System.Collections.Generic;
    using Blocks;
    using JumpKing.API;
    using JumpKing.Level;
    using JumpKing.Level.Sampler;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Factory for sand blocks.
    /// </summary>
    public class FactorySand : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.SandOn,
            ModBlocks.SandOff,
            ModBlocks.SandLever,
            ModBlocks.SandLeverOn,
            ModBlocks.SandLeverOff,
            ModBlocks.SandLeverSolid,
            ModBlocks.SandLeverSolidOn,
            ModBlocks.SandLeverSolidOff
        };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.SandOn:
                case var _ when blockCode == ModBlocks.SandOff:
                case var _ when blockCode == ModBlocks.SandLeverSolid:
                case var _ when blockCode == ModBlocks.SandLeverSolidOn:
                case var _ when blockCode == ModBlocks.SandLeverSolidOff:
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.SandOn:
                    return new BlockSandOn(blockRect);
                case var _ when blockCode == ModBlocks.SandOff:
                    return new BlockSandOff(blockRect);
                case var _ when blockCode == ModBlocks.SandLever:
                    return new BlockSandLever(blockRect);
                case var _ when blockCode == ModBlocks.SandLeverOn:
                    return new BlockSandLeverOn(blockRect);
                case var _ when blockCode == ModBlocks.SandLeverOff:
                    return new BlockSandLeverOff(blockRect);
                case var _ when blockCode == ModBlocks.SandLeverSolid:
                    return new BlockSandLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.SandLeverSolidOn:
                    return new BlockSandLeverSolidOn(blockRect);
                case var _ when blockCode == ModBlocks.SandLeverSolidOff:
                    return new BlockSandLeverSolidOff(blockRect);
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactorySand)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
