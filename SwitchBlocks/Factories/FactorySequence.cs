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
    ///     Factory for sequence blocks.
    /// </summary>
    public class FactorySequence : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.SequenceA,
            ModBlocks.SequenceB,
            ModBlocks.SequenceC,
            ModBlocks.SequenceD,
            ModBlocks.SequenceIceA,
            ModBlocks.SequenceIceB,
            ModBlocks.SequenceIceC,
            ModBlocks.SequenceIceD,
            ModBlocks.SequenceSnowA,
            ModBlocks.SequenceSnowB,
            ModBlocks.SequenceSnowC,
            ModBlocks.SequenceSnowD,
            ModBlocks.SequenceSlopeA,
            ModBlocks.SequenceSlopeB,
            ModBlocks.SequenceSlopeC,
            ModBlocks.SequenceSlopeD,
            ModBlocks.SequenceReset,
            ModBlocks.SequenceResetSolid,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidSequenceBlocks = new HashSet<Color>
        {
            ModBlocks.SequenceA,
            ModBlocks.SequenceB,
            ModBlocks.SequenceC,
            ModBlocks.SequenceD,
            ModBlocks.SequenceIceA,
            ModBlocks.SequenceIceB,
            ModBlocks.SequenceIceC,
            ModBlocks.SequenceIceD,
            ModBlocks.SequenceSnowA,
            ModBlocks.SequenceSnowB,
            ModBlocks.SequenceSnowC,
            ModBlocks.SequenceSnowD,
            ModBlocks.SequenceSlopeA,
            ModBlocks.SequenceSlopeB,
            ModBlocks.SequenceSlopeC,
            ModBlocks.SequenceSlopeD,
            ModBlocks.SequenceResetSolid,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>>
            {
                // Sequence A
                [ModBlocks.SequenceA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceA(rect);
                    SetupSequence.BlocksSequenceA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceB(rect);
                    SetupSequence.BlocksSequenceB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceC(rect);
                    SetupSequence.BlocksSequenceC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceD(rect);
                    SetupSequence.BlocksSequenceD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },

                // Sequence Ice
                [ModBlocks.SequenceIceA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceIceA(rect);
                    SetupSequence.BlocksSequenceA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceIceB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceIceB(rect);
                    SetupSequence.BlocksSequenceB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceIceC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceIceC(rect);
                    SetupSequence.BlocksSequenceC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceIceD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceIceD(rect);
                    SetupSequence.BlocksSequenceD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },

                // Sequence Snow
                [ModBlocks.SequenceSnowA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSnowA(rect);
                    SetupSequence.BlocksSequenceA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceSnowB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSnowB(rect);
                    SetupSequence.BlocksSequenceB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceSnowC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSnowC(rect);
                    SetupSequence.BlocksSequenceC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceSnowD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSnowD(rect);
                    SetupSequence.BlocksSequenceD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },

                // Sequence Slope
                [ModBlocks.SequenceSlopeA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSlopeA(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupSequence.BlocksSequenceA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceSlopeB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSlopeB(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupSequence.BlocksSequenceB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceSlopeC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSlopeC(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupSequence.BlocksSequenceC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceSlopeD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceSlopeD(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupSequence.BlocksSequenceD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },

                // Resets
                [ModBlocks.SequenceReset] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceReset(rect);
                    SetupSequence.Resets[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.SequenceResetSolid] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockSequenceResetSolid(rect);
                    SetupSequence.Resets[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode) => SolidSequenceBlocks.Contains(blockCode);

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID)
            {
                SetupSequence.BlocksSequenceA.Clear();
                SetupSequence.BlocksSequenceB.Clear();
                SetupSequence.BlocksSequenceC.Clear();
                SetupSequence.BlocksSequenceD.Clear();
                SetupSequence.Resets.Clear();
                LastUsedMapId = level.ID;
            }

            if (BlockFactories.TryGetValue(blockCode, out var factory))
            {
                return factory(blockRect, textureSrc, currentScreen, x, y);
            }

            throw new InvalidOperationException(
                $"{nameof(FactorySequence)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }
    }
}
