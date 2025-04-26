namespace SwitchBlocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Blocks;

    /// <summary>
    /// Collection of blockcolors that are used by the mod.
    /// </summary>
    public static class ModBlocks
    {
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoOn"/>.</summary>
        public static readonly Color AUTO_ON = new Color(238, 124, 10);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoOff"/>.</summary>
        public static readonly Color AUTO_OFF = new Color(10, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoIceOn"/>.</summary>
        public static readonly Color AUTO_ICE_ON = new Color(238, 124, 11);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoIceOff"/>.</summary>
        public static readonly Color AUTO_ICE_OFF = new Color(11, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoSnowOn"/>.</summary>
        public static readonly Color AUTO_SNOW_ON = new Color(238, 124, 12);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoSnowOff"/>.</summary>
        public static readonly Color AUTO_SNOW_OFF = new Color(12, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoReset"/>.</summary>
        public static readonly Color AUTO_RESET = new Color(238, 11, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockAutoResetFull"/>.</summary>
        public static readonly Color AUTO_RESET_FULL = new Color(238, 12, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockWind"/>. Enabling wind on the screen its placed.</summary>
        public static readonly Color AUTO_WIND_ENABLE = new Color(238, 17, 124);

        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicOn"/>.</summary>
        public static readonly Color BASIC_ON = new Color(238, 124, 20);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicOff"/>.</summary>
        public static readonly Color BASIC_OFF = new Color(20, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicIceOn"/>.</summary>
        public static readonly Color BASIC_ICE_ON = new Color(238, 124, 21);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicIceOff"/>.</summary>
        public static readonly Color BASIC_ICE_OFF = new Color(21, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicSnowOn"/>.</summary>
        public static readonly Color BASIC_SNOW_ON = new Color(238, 124, 22);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicSnowOff"/>.</summary>
        public static readonly Color BASIC_SNOW_OFF = new Color(22, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicLever"/>.</summary>
        public static readonly Color BASIC_LEVER = new Color(238, 21, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicLeverOn"/>, that can only turn the state on.</summary>
        public static readonly Color BASIC_LEVER_ON = new Color(238, 22, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicLeverOff"/>, that can only turn the state off.</summary>
        public static readonly Color BASIC_LEVER_OFF = new Color(238, 23, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicLeverSolid"/>.</summary>
        public static readonly Color BASIC_LEVER_SOLID = new Color(238, 24, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicLeverSolidOn"/>, that can only turn the state on.</summary>
        public static readonly Color BASIC_LEVER_SOLID_ON = new Color(238, 25, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockBasicLeverSolidOff"/>, that can only turn the state off.</summary>
        public static readonly Color BASIC_LEVER_SOLID_OFF = new Color(238, 26, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockWind"/>. Enabling wind on the screen its placed.</summary>
        public static readonly Color BASIC_WIND_ENABLE = new Color(238, 27, 124);

        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownOn"/>.</summary>
        public static readonly Color COUNTDOWN_ON = new Color(238, 124, 30);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownOff"/>.</summary>
        public static readonly Color COUNTDOWN_OFF = new Color(30, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownIceOn"/>.</summary>
        public static readonly Color COUNTDOWN_ICE_ON = new Color(238, 124, 31);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownIceOff"/>.</summary>
        public static readonly Color COUNTDOWN_ICE_OFF = new Color(31, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownSnowOn"/>.</summary>
        public static readonly Color COUNTDOWN_SNOW_ON = new Color(238, 124, 32);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownSnowOff"/>.</summary>
        public static readonly Color COUNTDOWN_SNOW_OFF = new Color(32, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownLever"/>.</summary>
        public static readonly Color COUNTDOWN_LEVER = new Color(238, 31, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownSingleUse"/>.</summary>
        public static readonly Color COUNTDOWN_SINGLE_USE = new Color(238, 32, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownLeverSolid"/>.</summary>
        public static readonly Color COUNTDOWN_LEVER_SOLID = new Color(238, 34, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockCountdownSingleUseSolid"/>.</summary>
        public static readonly Color COUNTDOWN_SINGLE_USE_SOLID = new Color(238, 35, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockWind"/>. Enabling wind on the screen its placed.</summary>
        public static readonly Color COUNTDOWN_WIND_ENABLE = new Color(238, 37, 124);

        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupA"/>.</summary>
        public static readonly Color GROUP_A = new Color(238, 124, 50);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupB"/>.</summary>
        public static readonly Color GROUP_B = new Color(50, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupC"/>.</summary>
        public static readonly Color GROUP_C = new Color(124, 238, 50);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupD"/>.</summary>
        public static readonly Color GROUP_D = new Color(50, 238, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupIceA"/>.</summary>
        public static readonly Color GROUP_ICE_A = new Color(238, 124, 51);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupIceB"/>.</summary>
        public static readonly Color GROUP_ICE_B = new Color(51, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupIceC"/>.</summary>
        public static readonly Color GROUP_ICE_C = new Color(124, 238, 51);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupIceD"/>.</summary>
        public static readonly Color GROUP_ICE_D = new Color(51, 238, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupSnowA"/>.</summary>
        public static readonly Color GROUP_SNOW_A = new Color(238, 124, 52);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupSnowB"/>.</summary>
        public static readonly Color GROUP_SNOW_B = new Color(52, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupSnowC"/>.</summary>
        public static readonly Color GROUP_SNOW_C = new Color(124, 238, 52);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupSnowD"/>.</summary>
        public static readonly Color GROUP_SNOW_D = new Color(52, 238, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupReset"/>.</summary>
        public static readonly Color GROUP_RESET = new Color(238, 51, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockGroupResetSolid"/>.</summary>
        public static readonly Color GROUP_RESET_SOLID = new Color(238, 54, 124);

        /// <summary><see cref="Color"/> that represents the <see cref="BlockJumpOn"/>.</summary>
        public static readonly Color JUMP_ON = new Color(31, 31, 31);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockJumpOff"/>.</summary>
        public static readonly Color JUMP_OFF = new Color(95, 95, 95);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockJumpIceOn"/>.</summary>
        public static readonly Color JUMP_ICE_ON = new Color(31, 32, 31);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockJumpIceOff"/>.</summary>
        public static readonly Color JUMP_ICE_OFF = new Color(95, 96, 95);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockJumpSnowOn"/>.</summary>
        public static readonly Color JUMP_SNOW_ON = new Color(31, 33, 31);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockJumpSnowOff"/>.</summary>
        public static readonly Color JUMP_SNOW_OFF = new Color(95, 97, 95);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockWind"/>. Enabling wind on the screen its placed.</summary>
        public static readonly Color JUMP_WIND_ENABLE = new Color(95, 95, 96);

        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandOn"/>.</summary>
        public static readonly Color SAND_ON = new Color(238, 124, 40);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandOff"/>.</summary>
        public static readonly Color SAND_OFF = new Color(40, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandLever"/>.</summary>
        public static readonly Color SAND_LEVER = new Color(238, 41, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandLeverOn"/>, that can only turn the state on.</summary>
        public static readonly Color SAND_LEVER_ON = new Color(238, 42, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandLeverOff"/>, that can only turn the state off.</summary>
        public static readonly Color SAND_LEVER_OFF = new Color(238, 43, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandLeverSolid"/>.</summary>
        public static readonly Color SAND_LEVER_SOLID = new Color(238, 44, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandLeverSolidOn"/>, that can only turn the state on.</summary>
        public static readonly Color SAND_LEVER_SOLID_ON = new Color(238, 45, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSandLeverSolidOff"/>, that can only turn the state off.</summary>
        public static readonly Color SAND_LEVER_SOLID_OFF = new Color(238, 46, 124);

        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceA"/>.</summary>
        public static readonly Color SEQUENCE_A = new Color(238, 124, 60);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceB"/>.</summary>
        public static readonly Color SEQUENCE_B = new Color(60, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceC"/>.</summary>
        public static readonly Color SEQUENCE_C = new Color(124, 238, 60);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceD"/>.</summary>
        public static readonly Color SEQUENCE_D = new Color(60, 238, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceIceA"/>.</summary>
        public static readonly Color SEQUENCE_ICE_A = new Color(238, 124, 61);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceIceB"/>.</summary>
        public static readonly Color SEQUENCE_ICE_B = new Color(61, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceIceC"/>.</summary>
        public static readonly Color SEQUENCE_ICE_C = new Color(124, 238, 61);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceIceD"/>.</summary>
        public static readonly Color SEQUENCE_ICE_D = new Color(61, 238, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceSnowA"/>.</summary>
        public static readonly Color SEQUENCE_SNOW_A = new Color(238, 124, 62);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceSnowB"/>.</summary>
        public static readonly Color SEQUENCE_SNOW_B = new Color(62, 124, 238);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceSnowC"/>.</summary>
        public static readonly Color SEQUENCE_SNOW_C = new Color(124, 238, 62);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceSnowD"/>.</summary>
        public static readonly Color SEQUENCE_SNOW_D = new Color(62, 238, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceReset"/>.</summary>
        public static readonly Color SEQUENCE_RESET = new Color(238, 61, 124);
        /// <summary><see cref="Color"/> that represents the <see cref="BlockSequenceResetSolid"/>.</summary>
        public static readonly Color SEQUENCE_RESET_SOLID = new Color(238, 64, 124);
    }
}
