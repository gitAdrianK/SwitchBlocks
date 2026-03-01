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

    /// <summary>
    ///     Factory for sand blocks.
    /// </summary>
    public class FactorySand : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.SandOn,
            ModBlocks.SandOff,
            ModBlocks.SandLever,
            ModBlocks.SandLeverOn,
            ModBlocks.SandLeverOff,
            ModBlocks.SandLeverSolid,
            ModBlocks.SandLeverSolidOn,
            ModBlocks.SandLeverSolidOff,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidSandBlocks = new HashSet<Color>
        {
            ModBlocks.SandOn,
            ModBlocks.SandOff,
            ModBlocks.SandLeverSolid,
            ModBlocks.SandLeverSolidOn,
            ModBlocks.SandLeverSolidOff,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, IBlock>>
            {
                [ModBlocks.SandOn] = rect => new BlockSandOn(rect),
                [ModBlocks.SandOff] = rect => new BlockSandOff(rect),
                [ModBlocks.SandLever] = rect => new BlockSandLever(rect),
                [ModBlocks.SandLeverOn] = rect => new BlockSandLeverOn(rect),
                [ModBlocks.SandLeverOff] = rect => new BlockSandLeverOff(rect),
                [ModBlocks.SandLeverSolid] = rect => new BlockSandLeverSolid(rect),
                [ModBlocks.SandLeverSolidOn] = rect => new BlockSandLeverSolidOn(rect),
                [ModBlocks.SandLeverSolidOff] = rect => new BlockSandLeverSolidOff(rect),
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode) => SolidSandBlocks.Contains(blockCode);

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
                return factory(blockRect);
            }

            throw new InvalidOperationException(
                $"{nameof(FactorySand)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }
    }
}
