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
            ModBlocks.SequenceReset,
            ModBlocks.SequenceResetSolid
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
                case var _ when blockCode == ModBlocks.SequenceA:
                case var _ when blockCode == ModBlocks.SequenceB:
                case var _ when blockCode == ModBlocks.SequenceC:
                case var _ when blockCode == ModBlocks.SequenceD:
                case var _ when blockCode == ModBlocks.SequenceIceA:
                case var _ when blockCode == ModBlocks.SequenceIceB:
                case var _ when blockCode == ModBlocks.SequenceIceC:
                case var _ when blockCode == ModBlocks.SequenceIceD:
                case var _ when blockCode == ModBlocks.SequenceSnowA:
                case var _ when blockCode == ModBlocks.SequenceSnowB:
                case var _ when blockCode == ModBlocks.SequenceSnowC:
                case var _ when blockCode == ModBlocks.SequenceSnowD:
                case var _ when blockCode == ModBlocks.SequenceResetSolid:
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
                SetupSequence.BlocksSequenceA.Clear();
                SetupSequence.BlocksSequenceB.Clear();
                SetupSequence.BlocksSequenceC.Clear();
                SetupSequence.BlocksSequenceD.Clear();
                SetupSequence.Resets.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                // Position stored in a single integer.
                // X and Y can never be a three-digit number.
                // Screen can never be a four-digit number.
                // As such the integers form is 00...00SSSXXYY.
                case var _ when blockCode == ModBlocks.SequenceA:
                    var blockSequenceA = new BlockSequenceA(blockRect);
                    SetupSequence.BlocksSequenceA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceA;
                    return blockSequenceA;
                case var _ when blockCode == ModBlocks.SequenceB:
                    var blockSequenceB = new BlockSequenceB(blockRect);
                    SetupSequence.BlocksSequenceB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceB;
                    return blockSequenceB;
                case var _ when blockCode == ModBlocks.SequenceC:
                    var blockSequenceC = new BlockSequenceC(blockRect);
                    SetupSequence.BlocksSequenceC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceC;
                    return blockSequenceC;
                case var _ when blockCode == ModBlocks.SequenceD:
                    var blockSequenceD = new BlockSequenceD(blockRect);
                    SetupSequence.BlocksSequenceD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceD;
                    return blockSequenceD;
                case var _ when blockCode == ModBlocks.SequenceIceA:
                    var blockSequenceIceA = new BlockSequenceIceA(blockRect);
                    SetupSequence.BlocksSequenceA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceA;
                    return blockSequenceIceA;
                case var _ when blockCode == ModBlocks.SequenceIceB:
                    var blockSequenceIceB = new BlockSequenceIceB(blockRect);
                    SetupSequence.BlocksSequenceB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceB;
                    return blockSequenceIceB;
                case var _ when blockCode == ModBlocks.SequenceIceC:
                    var blockSequenceIceC = new BlockSequenceIceC(blockRect);
                    SetupSequence.BlocksSequenceC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceC;
                    return blockSequenceIceC;
                case var _ when blockCode == ModBlocks.SequenceIceD:
                    var blockSequenceIceD = new BlockSequenceIceD(blockRect);
                    SetupSequence.BlocksSequenceD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceD;
                    return blockSequenceIceD;
                case var _ when blockCode == ModBlocks.SequenceSnowA:
                    var blockSequenceSnowA = new BlockSequenceSnowA(blockRect);
                    SetupSequence.BlocksSequenceA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowA;
                    return blockSequenceSnowA;
                case var _ when blockCode == ModBlocks.SequenceSnowB:
                    var blockSequenceSnowB = new BlockSequenceSnowB(blockRect);
                    SetupSequence.BlocksSequenceB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowB;
                    return blockSequenceSnowB;
                case var _ when blockCode == ModBlocks.SequenceSnowC:
                    var blockSequenceSnowC = new BlockSequenceSnowC(blockRect);
                    SetupSequence.BlocksSequenceC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowC;
                    return blockSequenceSnowC;
                case var _ when blockCode == ModBlocks.SequenceSnowD:
                    var blockSequenceSnowD = new BlockSequenceSnowD(blockRect);
                    SetupSequence.BlocksSequenceD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowD;
                    return blockSequenceSnowD;
                case var _ when blockCode == ModBlocks.SequenceReset:
                    var blockSequenceReset = new BlockSequenceReset(blockRect);
                    SetupSequence.Resets[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceReset;
                    return blockSequenceReset;
                case var _ when blockCode == ModBlocks.SequenceResetSolid:
                    var blockSequenceResetSolid = new BlockSequenceResetSolid(blockRect);
                    SetupSequence.Resets[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceResetSolid;
                    return blockSequenceResetSolid;
                default:
                    throw new InvalidOperationException(
                        $"{nameof(FactorySequence)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
