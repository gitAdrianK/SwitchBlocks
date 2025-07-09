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
            ModBlocks.GroupReset,
            ModBlocks.GroupResetSolid
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
                case var _ when blockCode == ModBlocks.GroupA:
                case var _ when blockCode == ModBlocks.GroupB:
                case var _ when blockCode == ModBlocks.GroupC:
                case var _ when blockCode == ModBlocks.GroupD:
                case var _ when blockCode == ModBlocks.GroupIceA:
                case var _ when blockCode == ModBlocks.GroupIceB:
                case var _ when blockCode == ModBlocks.GroupIceC:
                case var _ when blockCode == ModBlocks.GroupIceD:
                case var _ when blockCode == ModBlocks.GroupSnowA:
                case var _ when blockCode == ModBlocks.GroupSnowB:
                case var _ when blockCode == ModBlocks.GroupSnowC:
                case var _ when blockCode == ModBlocks.GroupSnowD:
                case var _ when blockCode == ModBlocks.GroupResetSolid:
                    return true;
            }

            return false;
        }

        /// <inheritdoc />
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc,
            int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                SetupGroup.BlocksGroupA.Clear();
                SetupGroup.BlocksGroupB.Clear();
                SetupGroup.BlocksGroupC.Clear();
                SetupGroup.BlocksGroupD.Clear();
                SetupGroup.Resets.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                // Position stored in a single integer.
                // X and Y can never be a three digit number.
                // Screen can never be a four digit number.
                // As such the integers form is 00...00SSSXXYY.
                case var _ when blockCode == ModBlocks.GroupA:
                    var blockGroupA = new BlockGroupA(blockRect);
                    SetupGroup.BlocksGroupA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupA;
                    return blockGroupA;
                case var _ when blockCode == ModBlocks.GroupB:
                    var blockGroupB = new BlockGroupB(blockRect);
                    SetupGroup.BlocksGroupB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupB;
                    return blockGroupB;
                case var _ when blockCode == ModBlocks.GroupC:
                    var blockGroupC = new BlockGroupC(blockRect);
                    SetupGroup.BlocksGroupC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupC;
                    return blockGroupC;
                case var _ when blockCode == ModBlocks.GroupD:
                    var blockGroupD = new BlockGroupD(blockRect);
                    SetupGroup.BlocksGroupD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupD;
                    return blockGroupD;
                case var _ when blockCode == ModBlocks.GroupIceA:
                    var blockGroupIceA = new BlockGroupIceA(blockRect);
                    SetupGroup.BlocksGroupA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceA;
                    return blockGroupIceA;
                case var _ when blockCode == ModBlocks.GroupIceB:
                    var blockGroupIceB = new BlockGroupIceB(blockRect);
                    SetupGroup.BlocksGroupB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceB;
                    return blockGroupIceB;
                case var _ when blockCode == ModBlocks.GroupIceC:
                    var blockGroupIceC = new BlockGroupIceC(blockRect);
                    SetupGroup.BlocksGroupC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceC;
                    return blockGroupIceC;
                case var _ when blockCode == ModBlocks.GroupIceD:
                    var blockGroupIceD = new BlockGroupIceD(blockRect);
                    SetupGroup.BlocksGroupD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceD;
                    return blockGroupIceD;
                case var _ when blockCode == ModBlocks.GroupSnowA:
                    var blockGroupSnowA = new BlockGroupSnowA(blockRect);
                    SetupGroup.BlocksGroupA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowA;
                    return blockGroupSnowA;
                case var _ when blockCode == ModBlocks.GroupSnowB:
                    var blockGroupSnowB = new BlockGroupSnowB(blockRect);
                    SetupGroup.BlocksGroupB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowB;
                    return blockGroupSnowB;
                case var _ when blockCode == ModBlocks.GroupSnowC:
                    var blockGroupSnowC = new BlockGroupSnowC(blockRect);
                    SetupGroup.BlocksGroupC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowC;
                    return blockGroupSnowC;
                case var _ when blockCode == ModBlocks.GroupSnowD:
                    var blockGroupSnowD = new BlockGroupSnowD(blockRect);
                    SetupGroup.BlocksGroupD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowD;
                    return blockGroupSnowD;
                case var _ when blockCode == ModBlocks.GroupReset:
                    var blockReset = new BlockGroupReset(blockRect);
                    SetupGroup.Resets[((currentScreen + 1) * 10000) + (x * 100) + y] = blockReset;
                    return blockReset;
                case var _ when blockCode == ModBlocks.GroupResetSolid:
                    var blockResetSolid = new BlockGroupResetSolid(blockRect);
                    SetupGroup.Resets[((currentScreen + 1) * 10000) + (x * 100) + y] = blockResetSolid;
                    return blockResetSolid;
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactoryGroup)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
