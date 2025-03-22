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
    /// Factory for basic blocks.
    /// </summary>
    public class FactoryBasic : IBlockFactory
    {
        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            ModBlocks.BASIC_ON,
            ModBlocks.BASIC_OFF,
            ModBlocks.BASIC_ICE_ON,
            ModBlocks.BASIC_ICE_OFF,
            ModBlocks.BASIC_SNOW_ON,
            ModBlocks.BASIC_SNOW_OFF,
            ModBlocks.BASIC_LEVER,
            ModBlocks.BASIC_LEVER_ON,
            ModBlocks.BASIC_LEVER_OFF,
            ModBlocks.BASIC_LEVER_SOLID,
            ModBlocks.BASIC_LEVER_SOLID_ON,
            ModBlocks.BASIC_LEVER_SOLID_OFF,
            ModBlocks.BASIC_WIND_ENABLE,
        };

        /// <inheritdoc/>
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc/>
        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.BASIC_ON:
                case var _ when blockCode == ModBlocks.BASIC_OFF:
                case var _ when blockCode == ModBlocks.BASIC_ICE_ON:
                case var _ when blockCode == ModBlocks.BASIC_ICE_OFF:
                case var _ when blockCode == ModBlocks.BASIC_SNOW_ON:
                case var _ when blockCode == ModBlocks.BASIC_SNOW_OFF:
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID:
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_ON:
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_OFF:
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
                SetupBasic.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.BASIC_ON:
                    return new BlockBasicOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_OFF:
                    return new BlockBasicOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_ICE_ON:
                    return new BlockBasicIceOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_ICE_OFF:
                    return new BlockBasicIceOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_SNOW_ON:
                    return new BlockBasicSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_SNOW_OFF:
                    return new BlockBasicSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER:
                    return new BlockBasicLever(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_ON:
                    return new BlockBasicLeverOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_OFF:
                    return new BlockBasicLeverOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID:
                    return new BlockBasicLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_ON:
                    return new BlockBasicLeverSolidOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_OFF:
                    return new BlockBasicLeverSolidOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_WIND_ENABLE:
                    _ = SetupBasic.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException($"{nameof(FactoryBasic)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
