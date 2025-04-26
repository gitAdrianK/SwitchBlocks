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
    /// Factory for countdown blocks.
    /// </summary>
    public class FactoryCountdown : IBlockFactory
    {
        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            ModBlocks.COUNTDOWN_ON,
            ModBlocks.COUNTDOWN_OFF,
            ModBlocks.COUNTDOWN_ICE_ON,
            ModBlocks.COUNTDOWN_ICE_OFF,
            ModBlocks.COUNTDOWN_SNOW_ON,
            ModBlocks.COUNTDOWN_SNOW_OFF,
            ModBlocks.COUNTDOWN_LEVER,
            ModBlocks.COUNTDOWN_LEVER_SOLID,
            ModBlocks.COUNTDOWN_SINGLE_USE,
            ModBlocks.COUNTDOWN_SINGLE_USE_SOLID,
            ModBlocks.COUNTDOWN_WIND_ENABLE,
        };

        /// <inheritdoc/>
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc/>
        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.COUNTDOWN_ON:
                case var _ when blockCode == ModBlocks.COUNTDOWN_OFF:
                case var _ when blockCode == ModBlocks.COUNTDOWN_ICE_ON:
                case var _ when blockCode == ModBlocks.COUNTDOWN_ICE_OFF:
                case var _ when blockCode == ModBlocks.COUNTDOWN_SNOW_ON:
                case var _ when blockCode == ModBlocks.COUNTDOWN_SNOW_OFF:
                case var _ when blockCode == ModBlocks.COUNTDOWN_LEVER_SOLID:
                case var _ when blockCode == ModBlocks.COUNTDOWN_SINGLE_USE_SOLID:
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
                SetupCountdown.SingleUseLevers.Clear();
                SetupCountdown.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.COUNTDOWN_ON:
                    return new BlockCountdownOn(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_OFF:
                    return new BlockCountdownOff(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_ICE_ON:
                    return new BlockCountdownIceOn(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_ICE_OFF:
                    return new BlockCountdownIceOff(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_SNOW_ON:
                    return new BlockCountdownSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_SNOW_OFF:
                    return new BlockCountdownSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_LEVER:
                    return new BlockCountdownLever(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_LEVER_SOLID:
                    return new BlockCountdownLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_SINGLE_USE:
                    var blockSingleUse = new BlockCountdownSingleUse(blockRect);
                    SetupCountdown.SingleUseLevers[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSingleUse;
                    return blockSingleUse;
                case var _ when blockCode == ModBlocks.COUNTDOWN_SINGLE_USE_SOLID:
                    var blockSingleUseSolid = new BlockCountdownSingleUseSolid(blockRect);
                    SetupCountdown.SingleUseLevers[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSingleUseSolid;
                    return blockSingleUseSolid;
                case var _ when blockCode == ModBlocks.COUNTDOWN_WIND_ENABLE:
                    _ = SetupCountdown.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException($"{nameof(FactoryCountdown)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
