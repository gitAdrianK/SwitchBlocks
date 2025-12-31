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
    using Setups;

    /// <summary>
    ///     Factory for basic blocks.
    /// </summary>
    public class FactoryBasic : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.BasicOn,
            ModBlocks.BasicOff,
            ModBlocks.BasicIceOn,
            ModBlocks.BasicIceOff,
            ModBlocks.BasicSnowOn,
            ModBlocks.BasicSnowOff,
            ModBlocks.BasicWaterOn,
            ModBlocks.BasicWaterOff,
            ModBlocks.BasicMoveUpOn,
            ModBlocks.BasicMoveUpOff,
            ModBlocks.BasicInfinityJumpOn,
            ModBlocks.BasicInfinityJumpOff,
            ModBlocks.BasicLever,
            ModBlocks.BasicLeverOn,
            ModBlocks.BasicLeverOff,
            ModBlocks.BasicLeverSolid,
            ModBlocks.BasicLeverSolidOn,
            ModBlocks.BasicLeverSolidOff,
            ModBlocks.BasicWindEnable,
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
                case var _ when blockCode == ModBlocks.BasicOn:
                case var _ when blockCode == ModBlocks.BasicOff:
                case var _ when blockCode == ModBlocks.BasicIceOn:
                case var _ when blockCode == ModBlocks.BasicIceOff:
                case var _ when blockCode == ModBlocks.BasicSnowOn:
                case var _ when blockCode == ModBlocks.BasicSnowOff:
                case var _ when blockCode == ModBlocks.BasicLeverSolid:
                case var _ when blockCode == ModBlocks.BasicLeverSolidOn:
                case var _ when blockCode == ModBlocks.BasicLeverSolidOff:
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID)
            {
                SetupBasic.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.BasicOn:
                    return new BlockBasicOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicOff:
                    return new BlockBasicOff(blockRect);
                case var _ when blockCode == ModBlocks.BasicIceOn:
                    return new BlockBasicIceOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicIceOff:
                    return new BlockBasicIceOff(blockRect);
                case var _ when blockCode == ModBlocks.BasicSnowOn:
                    return new BlockBasicSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicSnowOff:
                    return new BlockBasicSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.BasicWaterOn:
                    return new BlockBasicWaterOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicWaterOff:
                    return new BlockBasicWaterOff(blockRect);

                case var _ when blockCode == ModBlocks.BasicMoveUpOn:
                    return new BlockBasicMoveUpOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicMoveUpOff:
                    return new BlockBasicMoveUpOff(blockRect);
                case var _ when blockCode == ModBlocks.BasicInfinityJumpOn:
                    return new BlockBasicInfinityJumpOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicInfinityJumpOff:
                    return new BlockBasicInfinityJumpOff(blockRect);

                case var _ when blockCode == ModBlocks.BasicLever:
                    return new BlockBasicLever(blockRect);
                case var _ when blockCode == ModBlocks.BasicLeverOn:
                    return new BlockBasicLeverOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicLeverOff:
                    return new BlockBasicLeverOff(blockRect);
                case var _ when blockCode == ModBlocks.BasicLeverSolid:
                    return new BlockBasicLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.BasicLeverSolidOn:
                    return new BlockBasicLeverSolidOn(blockRect);
                case var _ when blockCode == ModBlocks.BasicLeverSolidOff:
                    return new BlockBasicLeverSolidOff(blockRect);
                case var _ when blockCode == ModBlocks.BasicWindEnable:
                    _ = SetupBasic.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactoryBasic)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
