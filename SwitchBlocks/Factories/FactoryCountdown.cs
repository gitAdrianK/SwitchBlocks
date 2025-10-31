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
    ///     Factory for countdown blocks.
    /// </summary>
    public class FactoryCountdown : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.CountdownOn,
            ModBlocks.CountdownOff,
            ModBlocks.CountdownIceOn,
            ModBlocks.CountdownIceOff,
            ModBlocks.CountdownSnowOn,
            ModBlocks.CountdownSnowOff,
            ModBlocks.CountdownWaterOn,
            ModBlocks.CountdownWaterOff,
            ModBlocks.CountdownLever,
            ModBlocks.CountdownLeverSolid,
            ModBlocks.CountdownSingleUse,
            ModBlocks.CountdownSingleUseSolid,
            ModBlocks.CountdownWindEnable
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
                case var _ when blockCode == ModBlocks.CountdownOn:
                case var _ when blockCode == ModBlocks.CountdownOff:
                case var _ when blockCode == ModBlocks.CountdownIceOn:
                case var _ when blockCode == ModBlocks.CountdownIceOff:
                case var _ when blockCode == ModBlocks.CountdownSnowOn:
                case var _ when blockCode == ModBlocks.CountdownSnowOff:
                case var _ when blockCode == ModBlocks.CountdownLeverSolid:
                case var _ when blockCode == ModBlocks.CountdownSingleUseSolid:
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
                SetupCountdown.SingleUseLevers.Clear();
                SetupCountdown.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.CountdownOn:
                    return new BlockCountdownOn(blockRect);
                case var _ when blockCode == ModBlocks.CountdownOff:
                    return new BlockCountdownOff(blockRect);
                case var _ when blockCode == ModBlocks.CountdownIceOn:
                    return new BlockCountdownIceOn(blockRect);
                case var _ when blockCode == ModBlocks.CountdownIceOff:
                    return new BlockCountdownIceOff(blockRect);
                case var _ when blockCode == ModBlocks.CountdownSnowOn:
                    return new BlockCountdownSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.CountdownSnowOff:
                    return new BlockCountdownSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.CountdownWaterOn:
                    return new BlockCountdownWaterOn(blockRect);
                case var _ when blockCode == ModBlocks.CountdownWaterOff:
                    return new BlockCountdownWaterOff(blockRect);
                case var _ when blockCode == ModBlocks.CountdownLever:
                    return new BlockCountdownLever(blockRect);
                case var _ when blockCode == ModBlocks.CountdownLeverSolid:
                    return new BlockCountdownLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.CountdownSingleUse:
                    var blockSingleUse = new BlockCountdownSingleUse(blockRect);
                    SetupCountdown.SingleUseLevers[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSingleUse;
                    return blockSingleUse;
                case var _ when blockCode == ModBlocks.CountdownSingleUseSolid:
                    var blockSingleUseSolid = new BlockCountdownSingleUseSolid(blockRect);
                    SetupCountdown.SingleUseLevers[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSingleUseSolid;
                    return blockSingleUseSolid;
                case var _ when blockCode == ModBlocks.CountdownWindEnable:
                    _ = SetupCountdown.WindEnabled.Add(currentScreen);
                    return new BlockWind();
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactoryCountdown)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
