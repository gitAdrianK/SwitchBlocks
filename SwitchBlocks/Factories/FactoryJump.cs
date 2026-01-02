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
            ModBlocks.JumpOnLegacy,
            ModBlocks.JumpOff,
            ModBlocks.JumpOffLegacy,
            ModBlocks.JumpIceOn,
            ModBlocks.JumpIceOnLegacy,
            ModBlocks.JumpIceOff,
            ModBlocks.JumpIceOffLegacy,
            ModBlocks.JumpSnowOn,
            ModBlocks.JumpSnowOnLegacy,
            ModBlocks.JumpSnowOff,
            ModBlocks.JumpSnowOffLegacy,
            ModBlocks.JumpWaterOn,
            ModBlocks.JumpWaterOnLegacy,
            ModBlocks.JumpWaterOff,
            ModBlocks.JumpWaterOffLegacy,
            ModBlocks.JumpInfinityJumpOn,
            ModBlocks.JumpInfinityJumpOnLegacy,
            ModBlocks.JumpInfinityJumpOff,
            ModBlocks.JumpInfinityJumpOffLegacy,
            ModBlocks.JumpWindEnable,
            ModBlocks.JumpWindEnableLegacy,
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
                case var _ when blockCode == ModBlocks.JumpOnLegacy:
                case var _ when blockCode == ModBlocks.JumpOff:
                case var _ when blockCode == ModBlocks.JumpOffLegacy:
                case var _ when blockCode == ModBlocks.JumpIceOn:
                case var _ when blockCode == ModBlocks.JumpIceOnLegacy:
                case var _ when blockCode == ModBlocks.JumpIceOff:
                case var _ when blockCode == ModBlocks.JumpIceOffLegacy:
                case var _ when blockCode == ModBlocks.JumpSnowOn:
                case var _ when blockCode == ModBlocks.JumpSnowOnLegacy:
                case var _ when blockCode == ModBlocks.JumpSnowOff:
                case var _ when blockCode == ModBlocks.JumpSnowOffLegacy:
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
                case var _ when blockCode == ModBlocks.JumpOnLegacy:
                    return new BlockJumpOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpOff:
                case var _ when blockCode == ModBlocks.JumpOffLegacy:
                    return new BlockJumpOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpIceOn:
                case var _ when blockCode == ModBlocks.JumpIceOnLegacy:
                    return new BlockJumpIceOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpIceOff:
                case var _ when blockCode == ModBlocks.JumpIceOffLegacy:
                    return new BlockJumpIceOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpSnowOn:
                case var _ when blockCode == ModBlocks.JumpSnowOnLegacy:
                    return new BlockJumpSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpSnowOff:
                case var _ when blockCode == ModBlocks.JumpSnowOffLegacy:
                    return new BlockJumpSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpWaterOn:
                case var _ when blockCode == ModBlocks.JumpWaterOnLegacy:
                    return new BlockJumpWaterOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpWaterOff:
                case var _ when blockCode == ModBlocks.JumpWaterOffLegacy:
                    return new BlockJumpWaterOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpInfinityJumpOn:
                case var _ when blockCode == ModBlocks.JumpInfinityJumpOnLegacy:
                    return new BlockJumpInfinityJumpOn(blockRect);
                case var _ when blockCode == ModBlocks.JumpInfinityJumpOff:
                case var _ when blockCode == ModBlocks.JumpInfinityJumpOffLegacy:
                    return new BlockJumpInfinityJumpOff(blockRect);
                case var _ when blockCode == ModBlocks.JumpWindEnable:
                case var _ when blockCode == ModBlocks.JumpWindEnableLegacy:
                    _ = SetupJump.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactoryJump)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
