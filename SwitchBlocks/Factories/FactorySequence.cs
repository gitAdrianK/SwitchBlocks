namespace SwitchBlocks.Factories
{
    using System;
    using System.Collections.Generic;
    using JumpKing.API;
    using JumpKing.Level;
    using JumpKing.Level.Sampler;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Setups;

    /// <summary>
    /// Factory for sequence blocks.
    /// </summary>
    public class FactorySequence : IBlockFactory
    {
        /// <summary>Last maps <c>ulong</c> steam id a block has been created for.</summary>
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;
        /// <summary>Supported Block Codes.</summary>
        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            ModBlocks.SEQUENCE_A,
            ModBlocks.SEQUENCE_B,
            ModBlocks.SEQUENCE_C,
            ModBlocks.SEQUENCE_D,
            ModBlocks.SEQUENCE_ICE_A,
            ModBlocks.SEQUENCE_ICE_B,
            ModBlocks.SEQUENCE_ICE_C,
            ModBlocks.SEQUENCE_ICE_D,
            ModBlocks.SEQUENCE_SNOW_A,
            ModBlocks.SEQUENCE_SNOW_B,
            ModBlocks.SEQUENCE_SNOW_C,
            ModBlocks.SEQUENCE_SNOW_D,
            ModBlocks.SEQUENCE_RESET,
            ModBlocks.SEQUENCE_RESET_SOLID,
        };

        /// <inheritdoc/>
        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        /// <inheritdoc/>
        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.SEQUENCE_A:
                case var _ when blockCode == ModBlocks.SEQUENCE_B:
                case var _ when blockCode == ModBlocks.SEQUENCE_C:
                case var _ when blockCode == ModBlocks.SEQUENCE_D:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_A:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_B:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_C:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_D:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_A:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_B:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_C:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_D:
                case var _ when blockCode == ModBlocks.SEQUENCE_RESET_SOLID:
                    return true;
                default:
                    break;
            }
            return false;
        }

        /// <inheritdoc/>
        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                SetupSequence.BlocksSequenceA.Clear();
                SetupSequence.BlocksSequenceB.Clear();
                SetupSequence.BlocksSequenceC.Clear();
                SetupSequence.BlocksSequenceD.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                // Position stored in a single integer.
                // X and Y can never be a three digit number.
                // Screen can never be a four digit number.
                // As such the integers form is 00...00SSSXXYY.
                case var _ when blockCode == ModBlocks.SEQUENCE_A:
                    var blockSequenceA = new BlockSequenceA(blockRect);
                    SetupSequence.BlocksSequenceA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceA;
                    return blockSequenceA;
                case var _ when blockCode == ModBlocks.SEQUENCE_B:
                    var blockSequenceB = new BlockSequenceB(blockRect);
                    SetupSequence.BlocksSequenceB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceB;
                    return blockSequenceB;
                case var _ when blockCode == ModBlocks.SEQUENCE_C:
                    var blockSequenceC = new BlockSequenceC(blockRect);
                    SetupSequence.BlocksSequenceC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceC;
                    return blockSequenceC;
                case var _ when blockCode == ModBlocks.SEQUENCE_D:
                    var blockSequenceD = new BlockSequenceD(blockRect);
                    SetupSequence.BlocksSequenceD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceD;
                    return blockSequenceD;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_A:
                    var blockSequenceIceA = new BlockSequenceIceA(blockRect);
                    SetupSequence.BlocksSequenceA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceA;
                    return blockSequenceIceA;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_B:
                    var blockSequenceIceB = new BlockSequenceIceB(blockRect);
                    SetupSequence.BlocksSequenceB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceB;
                    return blockSequenceIceB;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_C:
                    var blockSequenceIceC = new BlockSequenceIceC(blockRect);
                    SetupSequence.BlocksSequenceC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceC;
                    return blockSequenceIceC;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_D:
                    var blockSequenceIceD = new BlockSequenceIceD(blockRect);
                    SetupSequence.BlocksSequenceD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceIceD;
                    return blockSequenceIceD;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_A:
                    var blockSequenceSnowA = new BlockSequenceSnowA(blockRect);
                    SetupSequence.BlocksSequenceA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowA;
                    return blockSequenceSnowA;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_B:
                    var blockSequenceSnowB = new BlockSequenceSnowB(blockRect);
                    SetupSequence.BlocksSequenceB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowB;
                    return blockSequenceSnowB;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_C:
                    var blockSequenceSnowC = new BlockSequenceSnowC(blockRect);
                    SetupSequence.BlocksSequenceC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowC;
                    return blockSequenceSnowC;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_D:
                    var blockSequenceSnowD = new BlockSequenceSnowD(blockRect);
                    SetupSequence.BlocksSequenceD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockSequenceSnowD;
                    return blockSequenceSnowD;
                case var _ when blockCode == ModBlocks.SEQUENCE_RESET:
                    return new BlockSequenceReset(blockRect);
                case var _ when blockCode == ModBlocks.SEQUENCE_RESET_SOLID:
                    return new BlockSequenceResetSolid(blockRect);
                default:
                    throw new InvalidOperationException($"{nameof(FactorySequence)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
