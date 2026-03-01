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
    using Util;

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
            ModBlocks.CountdownSandOn,
            ModBlocks.CountdownSandOff,
            ModBlocks.CountdownSlopeOn,
            ModBlocks.CountdownSlopeOff,
            ModBlocks.CountdownLever,
            ModBlocks.CountdownLeverSolid,
            ModBlocks.CountdownSingleUse,
            ModBlocks.CountdownSingleUseSolid,
            ModBlocks.CountdownWindEnable,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidCountdownBlocks = new HashSet<Color>
        {
            ModBlocks.CountdownOn,
            ModBlocks.CountdownOff,
            ModBlocks.CountdownIceOn,
            ModBlocks.CountdownIceOff,
            ModBlocks.CountdownSnowOn,
            ModBlocks.CountdownSnowOff,
            ModBlocks.CountdownSandOff,
            ModBlocks.CountdownSandOn,
            ModBlocks.CountdownSlopeOn,
            ModBlocks.CountdownSlopeOff,
            ModBlocks.CountdownLeverSolid,
            ModBlocks.CountdownSingleUseSolid,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>>
            {
                [ModBlocks.CountdownOn] = (rect, src, screen, x, y) => new BlockCountdownOn(rect),
                [ModBlocks.CountdownOff] = (rect, src, screen, x, y) => new BlockCountdownOff(rect),
                [ModBlocks.CountdownIceOn] = (rect, src, screen, x, y) => new BlockCountdownIceOn(rect),
                [ModBlocks.CountdownIceOff] = (rect, src, screen, x, y) => new BlockCountdownIceOff(rect),
                [ModBlocks.CountdownSnowOn] = (rect, src, screen, x, y) => new BlockCountdownSnowOn(rect),
                [ModBlocks.CountdownSnowOff] = (rect, src, screen, x, y) => new BlockCountdownSnowOff(rect),
                [ModBlocks.CountdownWaterOn] = (rect, src, screen, x, y) => new BlockCountdownWaterOn(rect),
                [ModBlocks.CountdownWaterOff] = (rect, src, screen, x, y) => new BlockCountdownWaterOff(rect),
                [ModBlocks.CountdownSandOn] = (rect, src, screen, x, y) => new BlockCountdownSandOn(rect),
                [ModBlocks.CountdownSandOff] = (rect, src, screen, x, y) => new BlockCountdownSandOff(rect),
                [ModBlocks.CountdownSlopeOn] = (rect, src, screen, x, y) =>
                    new BlockCountdownSlopeOn(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.CountdownSlopeOff] = (rect, src, screen, x, y) =>
                    new BlockCountdownSlopeOff(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.CountdownLever] = (rect, src, screen, x, y) => new BlockCountdownLever(rect),
                [ModBlocks.CountdownLeverSolid] = (rect, src, screen, x, y) => new BlockCountdownLeverSolid(rect),
                [ModBlocks.CountdownSingleUse] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockCountdownSingleUse(rect);
                    SetupCountdown.SingleUseLevers[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.CountdownSingleUseSolid] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockCountdownSingleUseSolid(rect);
                    SetupCountdown.SingleUseLevers[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.CountdownWindEnable] = (rect, src, screen, x, y) =>
                {
                    _ = SetupCountdown.WindEnabled.Add(screen);
                    return new BlockWind();
                },
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level) =>
            SupportedBlockCodes.Contains(blockCode)
            || IsCountdownConveyorOn(blockCode)
            || IsCountdownConveyorOff(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode) =>
            SolidCountdownBlocks.Contains(blockCode)
            || IsCountdownConveyorOn(blockCode)
            || IsCountdownConveyorOff(blockCode);

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

            if (BlockFactories.TryGetValue(blockCode, out var factory))
            {
                return factory(blockRect, textureSrc, currentScreen, x, y);
            }

            if (IsCountdownConveyorOn(blockCode))
            {
                return new BlockCountdownConveyorOn(blockRect, blockCode.B);
            }

            if (IsCountdownConveyorOff(blockCode))
            {
                return new BlockCountdownConveyorOff(blockRect, blockCode.R);
            }

            throw new InvalidOperationException(
                $"{nameof(FactoryCountdown)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }

        /// <summary>
        ///     Check if the block-code is that of a <see cref="BlockCountdownConveyorOn" />
        /// </summary>
        /// <param name="blockCode">The to check block code.</param>
        /// <returns><c>true</c> if it is a <see cref="BlockCountdownConveyorOn" /> valid color, <c>false</c> otherwise.</returns>
        private static bool IsCountdownConveyorOn(Color blockCode) =>
            blockCode.R == ModBlocks.CountdownConveyorOn.R
            && blockCode.G == ModBlocks.CountdownConveyorOn.G
            && blockCode.B >= 1
            && blockCode.B <= 30;

        /// <summary>
        ///     Check if the block-code is that of a <see cref="BlockCountdownConveyorOff" />
        /// </summary>
        /// <param name="blockCode">The to check block code.</param>
        /// <returns><c>true</c> if it is a <see cref="BlockCountdownConveyorOff" /> valid color, <c>false</c> otherwise.</returns>
        private static bool IsCountdownConveyorOff(Color blockCode) =>
            blockCode.G == ModBlocks.CountdownConveyorOff.G
            && blockCode.B == ModBlocks.CountdownConveyorOff.B
            && blockCode.R >= 1
            && blockCode.R <= 30;
    }
}
