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

    public class FactoryJump : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.JumpOn,
            ModBlocks.JumpOff,
            ModBlocks.JumpIceOn,
            ModBlocks.JumpIceOff,
            ModBlocks.JumpSnowOn,
            ModBlocks.JumpSnowOff,
            ModBlocks.JumpWaterOn,
            ModBlocks.JumpWaterOff,
            ModBlocks.JumpInfinityJumpOn,
            ModBlocks.JumpInfinityJumpOff,
            ModBlocks.JumpWindEnable,
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
                case var _ when blockCode == ModBlocks.JumpOn:
                case var _ when blockCode == ModBlocks.JumpOff:
                case var _ when blockCode == ModBlocks.JumpIceOn:
                case var _ when blockCode == ModBlocks.JumpIceOff:
                case var _ when blockCode == ModBlocks.JumpSnowOn:
                case var _ when blockCode == ModBlocks.JumpSnowOff:
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
                SetupJump.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.JumpOn:
                    return new BlockJumpOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpOff:
                    return new BlockJumpOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpIceOn:
                    return new BlockJumpIceOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpIceOff:
                    return new BlockJumpIceOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpSnowOn:
                    return new BlockJumpSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpSnowOff:
                    return new BlockJumpSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpWaterOn:
                    return new BlockJumpWaterOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpWaterOff:
                    return new BlockJumpWaterOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpInfinityJumpOn:
                    return new BlockJumpInfinityJumpOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpInfinityJumpOff:
                    return new BlockJumpInfinityJumpOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpWindEnable:
                    _ = SetupJump.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactoryJump)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
