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
    using Util;

    /// <summary>
    ///     Factory for threshold blocks.
    /// </summary>
    public class FactoryThreshold : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.ThresholdOn,
            ModBlocks.ThresholdOff,
            ModBlocks.ThresholdIceOn,
            ModBlocks.ThresholdIceOff,
            ModBlocks.ThresholdSnowOn,
            ModBlocks.ThresholdSnowOff,
            ModBlocks.ThresholdWaterOn,
            ModBlocks.ThresholdWaterOff,
            ModBlocks.ThresholdSandOn,
            ModBlocks.ThresholdSandOff,
            ModBlocks.ThresholdSlopeOn,
            ModBlocks.ThresholdSlopeOff,
            ModBlocks.ThresholdReset,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidBlocks = new HashSet<Color>
        {
            ModBlocks.ThresholdOn,
            ModBlocks.ThresholdOff,
            ModBlocks.ThresholdIceOn,
            ModBlocks.ThresholdIceOff,
            ModBlocks.ThresholdSnowOn,
            ModBlocks.ThresholdSnowOff,
            ModBlocks.ThresholdSandOn,
            ModBlocks.ThresholdSandOff,
            ModBlocks.ThresholdSlopeOn,
            ModBlocks.ThresholdSlopeOff,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>>
            {
                [ModBlocks.ThresholdOn] = (rect, src, screen, x, y) => new BlockThresholdOn(rect),
                [ModBlocks.ThresholdOff] = (rect, src, screen, x, y) => new BlockThresholdOff(rect),
                [ModBlocks.ThresholdIceOn] = (rect, src, screen, x, y) => new BlockThresholdIceOn(rect),
                [ModBlocks.ThresholdIceOff] = (rect, src, screen, x, y) => new BlockThresholdIceOff(rect),
                [ModBlocks.ThresholdSnowOn] = (rect, src, screen, x, y) => new BlockThresholdSnowOn(rect),
                [ModBlocks.ThresholdSnowOff] = (rect, src, screen, x, y) => new BlockThresholdSnowOff(rect),
                [ModBlocks.ThresholdWaterOn] = (rect, src, screen, x, y) => new BlockThresholdWaterOn(rect),
                [ModBlocks.ThresholdWaterOff] = (rect, src, screen, x, y) => new BlockThresholdWaterOff(rect),
                [ModBlocks.ThresholdSandOn] = (rect, src, screen, x, y) => new BlockThresholdSandOn(rect),
                [ModBlocks.ThresholdSandOff] = (rect, src, screen, x, y) => new BlockThresholdSandOff(rect),
                [ModBlocks.ThresholdSlopeOn] = (rect, src, screen, x, y) =>
                    new BlockThresholdSlopeOn(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.ThresholdSlopeOff] = (rect, src, screen, x, y) =>
                    new BlockThresholdSlopeOff(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.ThresholdReset] = (rect, src, screen, x, y) => new BlockThresholdReset(rect),
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level)
            => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode)
            => SolidBlocks.Contains(blockCode);

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID)
            {
                LastUsedMapId = level.ID;
            }

            if (BlockFactories.TryGetValue(blockCode, out var factory))
            {
                return factory(blockRect, textureSrc, currentScreen, x, y);
            }

            throw new InvalidOperationException(
                $"{nameof(FactoryThreshold)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }
    }
}
