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
    ///     Factory for basic blocks.
    /// </summary>
    public class FactoryBasic : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.BasicOn,
            ModBlocks.BasicOff,
            ModBlocks.BasicIceOn,
            ModBlocks.BasicIceOff,
            ModBlocks.BasicSnowOn,
            ModBlocks.BasicSnowOff,
            ModBlocks.BasicWaterOn,
            ModBlocks.BasicWaterOff,
            ModBlocks.BasicSandOn,
            ModBlocks.BasicSandOff,
            ModBlocks.BasicSlopeOn,
            ModBlocks.BasicSlopeOff,
            ModBlocks.BasicMoveUpOn,
            ModBlocks.BasicMoveUpOff,
            ModBlocks.BasicInfinityJumpOn,
            ModBlocks.BasicInfinityJumpOff,
            ModBlocks.BasicLever,
            ModBlocks.BasicLeverOn,
            ModBlocks.BasicLeverOff,
            ModBlocks.BasicLeverSolid,
            ModBlocks.BasicLeverSolidOn,
            ModBlocks.BasicLeverSolidOff,
            ModBlocks.BasicWindEnable,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidBasicBlocks = new HashSet<Color>
        {
            ModBlocks.BasicOn,
            ModBlocks.BasicOff,
            ModBlocks.BasicIceOn,
            ModBlocks.BasicIceOff,
            ModBlocks.BasicSnowOn,
            ModBlocks.BasicSnowOff,
            ModBlocks.BasicSandOn,
            ModBlocks.BasicSandOff,
            ModBlocks.BasicSlopeOn,
            ModBlocks.BasicSlopeOff,
            ModBlocks.BasicLeverSolid,
            ModBlocks.BasicLeverSolidOn,
            ModBlocks.BasicLeverSolidOff,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>>
            {
                [ModBlocks.BasicOn] = (rect, src, screen, x, y) => new BlockBasicOn(rect),
                [ModBlocks.BasicOff] = (rect, src, screen, x, y) => new BlockBasicOff(rect),
                [ModBlocks.BasicIceOn] = (rect, src, screen, x, y) => new BlockBasicIceOn(rect),
                [ModBlocks.BasicIceOff] = (rect, src, screen, x, y) => new BlockBasicIceOff(rect),
                [ModBlocks.BasicSnowOn] = (rect, src, screen, x, y) => new BlockBasicSnowOn(rect),
                [ModBlocks.BasicSnowOff] = (rect, src, screen, x, y) => new BlockBasicSnowOff(rect),
                [ModBlocks.BasicWaterOn] = (rect, src, screen, x, y) => new BlockBasicWaterOn(rect),
                [ModBlocks.BasicWaterOff] = (rect, src, screen, x, y) => new BlockBasicWaterOff(rect),
                [ModBlocks.BasicSandOn] = (rect, src, screen, x, y) => new BlockBasicSandOn(rect),
                [ModBlocks.BasicSandOff] = (rect, src, screen, x, y) => new BlockBasicSandOff(rect),
                [ModBlocks.BasicSlopeOn] = (rect, src, screen, x, y) =>
                    new BlockBasicSlopeOn(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.BasicSlopeOff] = (rect, src, screen, x, y) =>
                    new BlockBasicSlopeOff(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.BasicMoveUpOn] = (rect, src, screen, x, y) => new BlockBasicMoveUpOn(rect),
                [ModBlocks.BasicMoveUpOff] = (rect, src, screen, x, y) => new BlockBasicMoveUpOff(rect),
                [ModBlocks.BasicInfinityJumpOn] = (rect, src, screen, x, y) => new BlockBasicInfinityJumpOn(rect),
                [ModBlocks.BasicInfinityJumpOff] = (rect, src, screen, x, y) => new BlockBasicInfinityJumpOff(rect),
                [ModBlocks.BasicLever] = (rect, src, screen, x, y) => new BlockBasicLever(rect),
                [ModBlocks.BasicLeverOn] = (rect, src, screen, x, y) => new BlockBasicLeverOn(rect),
                [ModBlocks.BasicLeverOff] = (rect, src, screen, x, y) => new BlockBasicLeverOff(rect),
                [ModBlocks.BasicLeverSolid] = (rect, src, screen, x, y) => new BlockBasicLeverSolid(rect),
                [ModBlocks.BasicLeverSolidOn] = (rect, src, screen, x, y) => new BlockBasicLeverSolidOn(rect),
                [ModBlocks.BasicLeverSolidOff] = (rect, src, screen, x, y) => new BlockBasicLeverSolidOff(rect),
                [ModBlocks.BasicWindEnable] = (rect, src, screen, x, y) =>
                {
                    _ = SetupBasic.WindEnabled.Add(screen);
                    return new BlockWind();
                },
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level) =>
            SupportedBlockCodes.Contains(blockCode)
            || IsBasicConveyorOn(blockCode)
            || IsBasicConveyorOff(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode) =>
            SolidBasicBlocks.Contains(blockCode)
            || IsBasicConveyorOn(blockCode)
            || IsBasicConveyorOff(blockCode);

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID)
            {
                SetupBasic.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            if (BlockFactories.TryGetValue(blockCode, out var factory))
            {
                return factory(blockRect, textureSrc, currentScreen, x, y);
            }

            if (IsBasicConveyorOn(blockCode))
            {
                return new BlockBasicConveyorOn(blockRect, blockCode.B);
            }

            if (IsBasicConveyorOff(blockCode))
            {
                return new BlockBasicConveyorOff(blockRect, blockCode.R);
            }

            throw new InvalidOperationException(
                $"{nameof(FactoryBasic)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }

        /// <summary>
        ///     Check if the block-code is that of a <see cref="BlockBasicConveyorOn" />
        /// </summary>
        /// <param name="blockCode">The to check block code.</param>
        /// <returns><c>true</c> if it is a <see cref="BlockBasicConveyorOn" /> valid color, <c>false</c> otherwise.</returns>
        private static bool IsBasicConveyorOn(Color blockCode) =>
            blockCode.R == ModBlocks.BasicConveyorOn.R
            && blockCode.G == ModBlocks.BasicConveyorOn.G
            && blockCode.B >= 1
            && blockCode.B <= 30;

        /// <summary>
        ///     Check if the block-code is that of a <see cref="BlockBasicConveyorOff" />
        /// </summary>
        /// <param name="blockCode">The to check block code.</param>
        /// <returns><c>true</c> if it is a <see cref="BlockBasicConveyorOff" /> valid color, <c>false</c> otherwise.</returns>
        private static bool IsBasicConveyorOff(Color blockCode) =>
            blockCode.G == ModBlocks.BasicConveyorOff.G
            && blockCode.B == ModBlocks.BasicConveyorOff.B
            && blockCode.R >= 1
            && blockCode.R <= 30;
    }
}
