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

    public class FactoryJump : IBlockFactory
    {
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color>
        {
            ModBlocks.JumpOn,
            ModBlocks.JumpOnLegacy,
            ModBlocks.JumpOff,
            ModBlocks.JumpOffLegacy,
            ModBlocks.JumpIceOn,
            ModBlocks.JumpIceOnLegacy,
            ModBlocks.JumpIceOff,
            ModBlocks.JumpIceOffLegacy,
            ModBlocks.JumpSnowOn,
            ModBlocks.JumpSnowOnLegacy,
            ModBlocks.JumpSnowOff,
            ModBlocks.JumpSnowOffLegacy,
            ModBlocks.JumpWaterOn,
            ModBlocks.JumpWaterOnLegacy,
            ModBlocks.JumpWaterOff,
            ModBlocks.JumpWaterOffLegacy,
            ModBlocks.JumpSandOn,
            ModBlocks.JumpSandOff,
            ModBlocks.JumpSlopeOn,
            ModBlocks.JumpSlopeOff,
            ModBlocks.JumpInfinityJumpOn,
            ModBlocks.JumpInfinityJumpOnLegacy,
            ModBlocks.JumpInfinityJumpOff,
            ModBlocks.JumpInfinityJumpOffLegacy,
            ModBlocks.JumpWindEnable,
            ModBlocks.JumpWindEnableLegacy,
        };

        /// <summary>Solid Block Codes.</summary>
        private static readonly HashSet<Color> SolidJumpBlocks = new HashSet<Color>
        {
            ModBlocks.JumpOn,
            ModBlocks.JumpOnLegacy,
            ModBlocks.JumpOff,
            ModBlocks.JumpOffLegacy,
            ModBlocks.JumpIceOn,
            ModBlocks.JumpIceOnLegacy,
            ModBlocks.JumpIceOff,
            ModBlocks.JumpIceOffLegacy,
            ModBlocks.JumpSnowOn,
            ModBlocks.JumpSnowOnLegacy,
            ModBlocks.JumpSnowOff,
            ModBlocks.JumpSnowOffLegacy,
            ModBlocks.JumpSandOn,
            ModBlocks.JumpSandOff,
            ModBlocks.JumpSlopeOn,
            ModBlocks.JumpSlopeOff,
        };

        /// <summary>Dictionary mapping the block-code to a function to properly handle all the possible blocks.</summary>
        private static readonly Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>> BlockFactories
            = new Dictionary<Color, Func<Rectangle, LevelTexture, int, int, int, IBlock>>
            {
                [ModBlocks.JumpOn] = (rect, src, screen, x, y) => new BlockJumpOn(rect),
                [ModBlocks.JumpOnLegacy] = (rect, src, screen, x, y) => new BlockJumpOn(rect),
                [ModBlocks.JumpOff] = (rect, src, screen, x, y) => new BlockJumpOff(rect),
                [ModBlocks.JumpOffLegacy] = (rect, src, screen, x, y) => new BlockJumpOff(rect),
                [ModBlocks.JumpIceOn] = (rect, src, screen, x, y) => new BlockJumpIceOn(rect),
                [ModBlocks.JumpIceOnLegacy] = (rect, src, screen, x, y) => new BlockJumpIceOn(rect),
                [ModBlocks.JumpIceOff] = (rect, src, screen, x, y) => new BlockJumpIceOff(rect),
                [ModBlocks.JumpIceOffLegacy] = (rect, src, screen, x, y) => new BlockJumpIceOff(rect),
                [ModBlocks.JumpSnowOn] = (rect, src, screen, x, y) => new BlockJumpSnowOn(rect),
                [ModBlocks.JumpSnowOnLegacy] = (rect, src, screen, x, y) => new BlockJumpSnowOn(rect),
                [ModBlocks.JumpSnowOff] = (rect, src, screen, x, y) => new BlockJumpSnowOff(rect),
                [ModBlocks.JumpSnowOffLegacy] = (rect, src, screen, x, y) => new BlockJumpSnowOff(rect),
                [ModBlocks.JumpWaterOn] = (rect, src, screen, x, y) => new BlockJumpWaterOn(rect),
                [ModBlocks.JumpWaterOnLegacy] = (rect, src, screen, x, y) => new BlockJumpWaterOn(rect),
                [ModBlocks.JumpWaterOff] = (rect, src, screen, x, y) => new BlockJumpWaterOff(rect),
                [ModBlocks.JumpWaterOffLegacy] = (rect, src, screen, x, y) => new BlockJumpWaterOff(rect),
                [ModBlocks.JumpSandOn] = (rect, src, screen, x, y) => new BlockJumpSandOn(rect),
                [ModBlocks.JumpSandOff] = (rect, src, screen, x, y) => new BlockJumpSandOff(rect),
                [ModBlocks.JumpSlopeOn] = (rect, src, screen, x, y) =>
                    new BlockJumpSlopeOn(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.JumpSlopeOff] = (rect, src, screen, x, y) =>
                    new BlockJumpSlopeOff(rect, Slopes.GetSlopeType(src, screen, x, y)),
                [ModBlocks.JumpInfinityJumpOn] = (rect, src, screen, x, y) => new BlockJumpInfinityJumpOn(rect),
                [ModBlocks.JumpInfinityJumpOnLegacy] = (rect, src, screen, x, y) => new BlockJumpInfinityJumpOn(rect),
                [ModBlocks.JumpInfinityJumpOff] = (rect, src, screen, x, y) => new BlockJumpInfinityJumpOff(rect),
                [ModBlocks.JumpInfinityJumpOffLegacy] = (rect, src, screen, x, y) => new BlockJumpInfinityJumpOff(rect),
                [ModBlocks.JumpWindEnable] = (rect, src, screen, x, y) =>
                {
                    _ = SetupJump.WindEnabled.Add(screen);
                    return new BlockWind();
                },
                [ModBlocks.JumpWindEnableLegacy] = (rect, src, screen, x, y) =>
                {
                    _ = SetupJump.WindEnabled.Add(screen);
                    return new BlockWind();
                },
            };

        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        /// <inheritdoc />
        public bool CanMakeBlock(Color blockCode, Level level)
            => SupportedBlockCodes.Contains(blockCode)
               || IsConveyorOn(blockCode)
               || IsConveyorOff(blockCode);

        /// <inheritdoc />
        public bool IsSolidBlock(Color blockCode)
            => SupportedBlockCodes.Contains(blockCode)
               || IsConveyorOn(blockCode)
               || IsConveyorOff(blockCode);

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID)
            {
                SetupJump.WindEnabled.Clear();
                LastUsedMapId = level.ID;
            }

            if (BlockFactories.TryGetValue(blockCode, out var factory))
            {
                return factory(blockRect, textureSrc, currentScreen, x, y);
            }

            if (IsConveyorOn(blockCode))
            {
                return new BlockJumpConveyorOn(blockRect, blockCode.B);
            }

            if (IsConveyorOff(blockCode))
            {
                return new BlockJumpConveyorOff(blockRect, blockCode.R);
            }

            throw new InvalidOperationException(
                $"{nameof(FactoryJump)} cannot create a block with Color ({blockCode.R}, {blockCode.G}, {blockCode.B})");
        }

        /// <summary>
        ///     Check if the block-code is that of a <see cref="BlockJumpConveyorOn" />
        /// </summary>
        /// <param name="blockCode">The block code to check.</param>
        /// <returns><c>true</c> if it is a <see cref="BlockJumpConveyorOn" /> valid color, <c>false</c> otherwise.</returns>
        private static bool IsConveyorOn(Color blockCode) =>
            blockCode.R == ModBlocks.JumpConveyorOn.R
            && blockCode.G == ModBlocks.JumpConveyorOn.G
            && blockCode.B >= 1
            && blockCode.B <= 30;

        /// <summary>
        ///     Check if the block-code is that of a <see cref="BlockJumpConveyorOff" />
        /// </summary>
        /// <param name="blockCode">The block code to check.</param>
        /// <returns><c>true</c> if it is a <see cref="BlockJumpConveyorOff" /> valid color, <c>false</c> otherwise.</returns>
        private static bool IsConveyorOff(Color blockCode) =>
            blockCode.G == ModBlocks.JumpConveyorOff.G
            && blockCode.B == ModBlocks.JumpConveyorOff.B
            && blockCode.R >= 1
            && blockCode.R <= 30;
    }
}
