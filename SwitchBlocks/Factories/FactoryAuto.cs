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
            ModBlocks.AutoSandOn,
            ModBlocks.AutoSandOff,
            ModBlocks.AutoSlopeOn,
            ModBlocks.AutoSlopeOff,
            ModBlocks.AutoReset,
            ModBlocks.AutoResetFull,
            ModBlocks.AutoWindEnable,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidAutoBlocks = new HashSet<Color>
        {
            ModBlocks.AutoOn,
            ModBlocks.AutoOff,
            ModBlocks.AutoIceOn,
            ModBlocks.AutoIceOff,
            ModBlocks.AutoSnowOn,
            ModBlocks.AutoSnowOff,
            ModBlocks.AutoSandOn,
            ModBlocks.AutoSandOff,
            ModBlocks.AutoSlopeOn,
            ModBlocks.AutoSlopeOff,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>>
            {
                [ModBlocks.AutoOn] = (rect, src, screen, x, y) => new BlockAutoOn(rect),
                [ModBlocks.AutoOff] = (rect, src, screen, x, y) => new BlockAutoOff(rect),
                [ModBlocks.AutoIceOn] = (rect, src, screen, x, y) => new BlockAutoIceOn(rect),
                [ModBlocks.AutoIceOff] = (rect, src, screen, x, y) => new BlockAutoIceOff(rect),
                [ModBlocks.AutoSnowOn] = (rect, src, screen, x, y) => new BlockAutoSnowOn(rect),
                [ModBlocks.AutoSnowOff] = (rect, src, screen, x, y) => new BlockAutoSnowOff(rect),
                [ModBlocks.AutoWaterOn] = (rect, src, screen, x, y) => new BlockAutoWaterOn(rect),
                [ModBlocks.AutoWaterOff] = (rect, src, screen, x, y) => new BlockAutoWaterOff(rect),
                [ModBlocks.AutoSandOn] = (rect, src, screen, x, y) => new BlockAutoSandOn(rect),
                [ModBlocks.AutoSandOff] = (rect, src, screen, x, y) => new BlockAutoSandOff(rect),
                [ModBlocks.AutoSlopeOn] = (rect, src, screen, x, y) =>
                    new BlockAutoSlopeOn(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.AutoSlopeOff] = (rect, src, screen, x, y) =>
                    new BlockAutoSlopeOff(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.AutoReset] = (rect, src, screen, x, y) => new BlockAutoReset(rect),
                [ModBlocks.AutoResetFull] = (rect, src, screen, x, y) => new BlockAutoResetFull(rect),
                [ModBlocks.AutoWindEnable] = (rect, src, screen, x, y) =>
                {
                    _ = SetupAuto.WindEnabled.Add(screen);
                    return new BlockWind();
                },
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode) => SolidAutoBlocks.Contains(blockCode);

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID)
            {
                SetupAuto.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            if (BlockFactories.TryGetValue(blockCode, out var factory))
            {
                return factory(blockRect, textureSrc, currentScreen, x, y);
            }

            throw new InvalidOperationException(
                $"{nameof(FactoryAuto)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }
    }
}
