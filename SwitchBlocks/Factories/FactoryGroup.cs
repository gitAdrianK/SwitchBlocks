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
    ///     Factory for group blocks.
    /// </summary>
    public class FactoryGroup : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.GroupA,
            ModBlocks.GroupB,
            ModBlocks.GroupC,
            ModBlocks.GroupD,
            ModBlocks.GroupIceA,
            ModBlocks.GroupIceB,
            ModBlocks.GroupIceC,
            ModBlocks.GroupIceD,
            ModBlocks.GroupSnowA,
            ModBlocks.GroupSnowB,
            ModBlocks.GroupSnowC,
            ModBlocks.GroupSnowD,
            ModBlocks.GroupSlopeA,
            ModBlocks.GroupSlopeB,
            ModBlocks.GroupSlopeC,
            ModBlocks.GroupSlopeD,
            ModBlocks.GroupReset,
            ModBlocks.GroupResetSolid,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidGroupBlocks = new HashSet<Color>
        {
            ModBlocks.GroupA,
            ModBlocks.GroupB,
            ModBlocks.GroupC,
            ModBlocks.GroupD,
            ModBlocks.GroupIceA,
            ModBlocks.GroupIceB,
            ModBlocks.GroupIceC,
            ModBlocks.GroupIceD,
            ModBlocks.GroupSnowA,
            ModBlocks.GroupSnowB,
            ModBlocks.GroupSnowC,
            ModBlocks.GroupSnowD,
            ModBlocks.GroupSlopeA,
            ModBlocks.GroupSlopeB,
            ModBlocks.GroupSlopeC,
            ModBlocks.GroupSlopeD,
            ModBlocks.GroupResetSolid,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>>
            {
                [ModBlocks.GroupA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupA(rect);
                    SetupGroup.BlocksGroupA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupB(rect);
                    SetupGroup.BlocksGroupB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupC(rect);
                    SetupGroup.BlocksGroupC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupD(rect);
                    SetupGroup.BlocksGroupD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupIceA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupIceA(rect);
                    SetupGroup.BlocksGroupA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupIceB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupIceB(rect);
                    SetupGroup.BlocksGroupB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupIceC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupIceC(rect);
                    SetupGroup.BlocksGroupC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupIceD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupIceD(rect);
                    SetupGroup.BlocksGroupD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSnowA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSnowA(rect);
                    SetupGroup.BlocksGroupA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSnowB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSnowB(rect);
                    SetupGroup.BlocksGroupB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSnowC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSnowC(rect);
                    SetupGroup.BlocksGroupC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSnowD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSnowD(rect);
                    SetupGroup.BlocksGroupD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSlopeA] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSlopeA(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupGroup.BlocksGroupA[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSlopeB] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSlopeB(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupGroup.BlocksGroupB[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSlopeC] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSlopeC(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupGroup.BlocksGroupC[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupSlopeD] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupSlopeD(rect, Slopes.GetSlopeType(src, screen, x, y));
                    SetupGroup.BlocksGroupD[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupReset] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupReset(rect);
                    SetupGroup.Resets[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
                [ModBlocks.GroupResetSolid] = (rect, src, screen, x, y) =>
                {
                    var b = new BlockGroupResetSolid(rect);
                    SetupGroup.Resets[((screen + 1) * 10000) + (x * 100) + y] = b;
                    return b;
                },
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode) => SolidGroupBlocks.Contains(blockCode);

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID)
            {
                SetupGroup.BlocksGroupA.Clear();
                SetupGroup.BlocksGroupB.Clear();
                SetupGroup.BlocksGroupC.Clear();
                SetupGroup.BlocksGroupD.Clear();
                SetupGroup.Resets.Clear();
                LastUsedMapId = level.ID;
            }

            if (BlockFactories.TryGetValue(blockCode, out var factory))
            {
                return factory(blockRect, textureSrc, currentScreen, x, y);
            }

            throw new InvalidOperationException(
                $"{nameof(FactoryGroup)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }
    }
}
