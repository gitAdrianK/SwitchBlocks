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
    /// Factory for auto blocks.
    /// </summary>
    public class FactoryAuto : IBlockFactory
    {
        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            ModBlocks.AUTO_ON,
            ModBlocks.AUTO_OFF,
            ModBlocks.AUTO_ICE_ON,
            ModBlocks.AUTO_ICE_OFF,
            ModBlocks.AUTO_SNOW_ON,
            ModBlocks.AUTO_SNOW_OFF,
            ModBlocks.AUTO_RESET,
            ModBlocks.AUTO_RESET_FULL,
            ModBlocks.AUTO_WIND_ENABLE,
        };

        /// <inheritdoc/>
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc/>
        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.AUTO_ON:
                case var _ when blockCode == ModBlocks.AUTO_OFF:
                case var _ when blockCode == ModBlocks.AUTO_ICE_ON:
                case var _ when blockCode == ModBlocks.AUTO_ICE_OFF:
                case var _ when blockCode == ModBlocks.AUTO_SNOW_ON:
                case var _ when blockCode == ModBlocks.AUTO_SNOW_OFF:
                    return true;
                default:
                    break;
            }
            return false;
        }

        /// <inheritdoc/>
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                SetupAuto.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.AUTO_ON:
                    return new BlockAutoOn(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_OFF:
                    return new BlockAutoOff(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_ICE_ON:
                    return new BlockAutoIceOn(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_ICE_OFF:
                    return new BlockAutoIceOff(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_SNOW_ON:
                    return new BlockAutoSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_SNOW_OFF:
                    return new BlockAutoSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_RESET:
                    return new BlockAutoReset(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_RESET_FULL:
                    return new BlockAutoResetFull(blockRect);
                case var _ when blockCode == ModBlocks.AUTO_WIND_ENABLE:
                    _ = SetupAuto.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException($"{nameof(FactoryAuto)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
