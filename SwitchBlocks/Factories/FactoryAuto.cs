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
    ///     Factory for auto blocks.
    /// </summary>
    public class FactoryAuto : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.AutoOn,
            ModBlocks.AutoOff,
            ModBlocks.AutoIceOn,
            ModBlocks.AutoIceOff,
            ModBlocks.AutoSnowOn,
            ModBlocks.AutoSnowOff,
            ModBlocks.AutoWaterOn,
            ModBlocks.AutoWaterOff,
            ModBlocks.AutoReset,
            ModBlocks.AutoResetFull,
            ModBlocks.AutoWindEnable
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
                case var _ when blockCode == ModBlocks.AutoOn:
                case var _ when blockCode == ModBlocks.AutoOff:
                case var _ when blockCode == ModBlocks.AutoIceOn:
                case var _ when blockCode == ModBlocks.AutoIceOff:
                case var _ when blockCode == ModBlocks.AutoSnowOn:
                case var _ when blockCode == ModBlocks.AutoSnowOff:
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
                SetupAuto.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.AutoOn:
                    return new BlockAutoOn(blockRect);
                case var _ when blockCode == ModBlocks.AutoOff:
                    return new BlockAutoOff(blockRect);
                case var _ when blockCode == ModBlocks.AutoIceOn:
                    return new BlockAutoIceOn(blockRect);
                case var _ when blockCode == ModBlocks.AutoIceOff:
                    return new BlockAutoIceOff(blockRect);
                case var _ when blockCode == ModBlocks.AutoSnowOn:
                    return new BlockAutoSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.AutoSnowOff:
                    return new BlockAutoSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.AutoWaterOn:
                    return new BlockAutoWaterOn(blockRect);
                case var _ when blockCode == ModBlocks.AutoWaterOff:
                    return new BlockAutoWaterOff(blockRect);
                case var _ when blockCode == ModBlocks.AutoReset:
                    return new BlockAutoReset(blockRect);
                case var _ when blockCode == ModBlocks.AutoResetFull:
                    return new BlockAutoResetFull(blockRect);
                case var _ when blockCode == ModBlocks.AutoWindEnable:
                    _ = SetupAuto.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactoryAuto)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
