using Microsoft.Xna.Framework;

namespace SwitchBlocks
{
    /// <summary>
    /// Contains colors used in this mod for its blocks as well as various other properties
    /// that can be set for the types of blocks, like durations and animation speed multiplier.
    /// </summary>
    public static class ModBlocks
    {
        /// <summary>
        /// Color that represents the auto on block. 
        /// </summary>
        public static readonly Color AUTO_ON = new Color(238, 124, 10);
        /// <summary>
        /// Color that represents the auto off block. 
        /// </summary>
        public static readonly Color AUTO_OFF = new Color(10, 124, 238);
        /// <summary>
        /// Color that represents the auto ice on block. 
        /// </summary>
        public static readonly Color AUTO_ICE_ON = new Color(238, 124, 11);
        /// <summary>
        /// Color that represents the auto ice off block. 
        /// </summary>
        public static readonly Color AUTO_ICE_OFF = new Color(11, 124, 238);
        /// <summary>
        /// Color that represents the auto snow on block. 
        /// </summary>
        public static readonly Color AUTO_SNOW_ON = new Color(238, 124, 12);
        /// <summary>
        /// Color that represents the auto snow off block. 
        /// </summary>
        public static readonly Color AUTO_SNOW_OFF = new Color(12, 124, 238);
        /// <summary>
        /// Color that represents the auto reset block. 
        /// </summary>
        public static readonly Color AUTO_RESET = new Color(238, 11, 124);
        /// <summary>
        /// Color that represents the auto reset block. 
        /// </summary>
        public static readonly Color AUTO_RESET_FULL = new Color(238, 12, 124);

        /// <summary>
        /// Color that represents the basic on block. 
        /// </summary>
        public static readonly Color BASIC_ON = new Color(238, 124, 20);
        /// <summary>
        /// Color that represents the basic off block. 
        /// </summary>
        public static readonly Color BASIC_OFF = new Color(20, 124, 238);
        /// <summary>
        /// Color that represents the basic ice on block. 
        /// </summary>
        public static readonly Color BASIC_ICE_ON = new Color(238, 124, 21);
        /// <summary>
        /// Color that represents the basic ice off block. 
        /// </summary>
        public static readonly Color BASIC_ICE_OFF = new Color(21, 124, 238);
        /// <summary>
        /// Color that represents the basic snow on block. 
        /// </summary>
        public static readonly Color BASIC_SNOW_ON = new Color(238, 124, 22);
        /// <summary>
        /// Color that represents the basic snow off block. 
        /// </summary>
        public static readonly Color BASIC_SNOW_OFF = new Color(22, 124, 238);
        /// <summary>
        /// Color that represents the basic solid lever block. 
        /// </summary>
        public static readonly Color BASIC_LEVER = new Color(238, 21, 124);
        /// <summary>
        /// Color that represents the basic lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color BASIC_LEVER_ON = new Color(238, 22, 124);
        /// <summary>
        /// Color that represents the basic lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color BASIC_LEVER_OFF = new Color(238, 23, 124);
        /// <summary>
        /// Color that represents the basic lever block. 
        /// </summary>
        public static readonly Color BASIC_LEVER_SOLID = new Color(238, 24, 124);
        /// <summary>
        /// Color that represents the basic solid lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color BASIC_LEVER_SOLID_ON = new Color(238, 25, 124);
        /// <summary>
        /// Color that represents the basic solid lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color BASIC_LEVER_SOLID_OFF = new Color(238, 26, 124);

        /// <summary>
        /// Color that represents the countdown on block. 
        /// </summary>
        public static readonly Color COUNTDOWN_ON = new Color(238, 124, 30);
        /// <summary>
        /// Color that represents the countdown off block. 
        /// </summary>
        public static readonly Color COUNTDOWN_OFF = new Color(30, 124, 238);
        /// <summary>
        /// Color that represents the countdown ice on block. 
        /// </summary>
        public static readonly Color COUNTDOWN_ICE_ON = new Color(238, 124, 31);
        /// <summary>
        /// Color that represents the countdown ice off block. 
        /// </summary>
        public static readonly Color COUNTDOWN_ICE_OFF = new Color(31, 124, 238);
        /// <summary>
        /// Color that represents the countdown snow on block. 
        /// </summary>
        public static readonly Color COUNTDOWN_SNOW_ON = new Color(238, 124, 32);
        /// <summary>
        /// Color that represents the countdown snow off block. 
        /// </summary>
        public static readonly Color COUNTDOWN_SNOW_OFF = new Color(32, 124, 238);
        /// <summary>
        /// Color that represents the countdown lever block. 
        /// </summary>
        public static readonly Color COUNTDOWN_LEVER = new Color(238, 31, 124);
        /// <summary>
        /// Color that represents the countdown solid lever block. 
        /// </summary>
        public static readonly Color COUNTDOWN_LEVER_SOLID = new Color(238, 34, 124);

        /// <summary>
        /// Color that represents the group A block. 
        /// </summary>
        public static readonly Color GROUP_A = new Color(238, 124, 50);
        /// <summary>
        /// Color that represents the group B block. 
        /// </summary>
        public static readonly Color GROUP_B = new Color(50, 124, 238);
        /// <summary>
        /// Color that represents the group C block. 
        /// </summary>
        public static readonly Color GROUP_C = new Color(124, 238, 50);
        /// <summary>
        /// Color that represents the group D block. 
        /// </summary>
        public static readonly Color GROUP_D = new Color(50, 238, 124);
        /// <summary>
        /// Color that represents the group ice A block. 
        /// </summary>
        public static readonly Color GROUP_ICE_A = new Color(238, 124, 51);
        /// <summary>
        /// Color that represents the group ice B block. 
        /// </summary>
        public static readonly Color GROUP_ICE_B = new Color(51, 124, 238);
        /// <summary>
        /// Color that represents the group ice C block. 
        /// </summary>
        public static readonly Color GROUP_ICE_C = new Color(124, 238, 51);
        /// <summary>
        /// Color that represents the group ice D block. 
        /// </summary>
        public static readonly Color GROUP_ICE_D = new Color(51, 238, 124);
        /// <summary>
        /// Color that represents the group snow A block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_A = new Color(238, 124, 52);
        /// <summary>
        /// Color that represents the group snow B block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_B = new Color(52, 124, 238);
        /// <summary>
        /// Color that represents the group snow C block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_C = new Color(124, 238, 52);
        /// <summary>
        /// Color that represents the group snow D block. 
        /// </summary>
        public static readonly Color GROUP_SNOW_D = new Color(52, 238, 124);
        /// <summary>
        /// Color that represents the group reset block. 
        /// </summary>
        public static readonly Color GROUP_RESET = new Color(238, 51, 124);
        /// <summary>
        /// Color that represents the countdown solid reset block. 
        /// </summary>
        public static readonly Color GROUP_RESET_SOLID = new Color(238, 54, 124);

        /// <summary>
        /// Color that represents the jump on block. 
        /// </summary>
        public static readonly Color JUMP_ON = new Color(31, 31, 31);
        /// <summary>
        /// Color that represents the jump off block. 
        /// </summary>
        public static readonly Color JUMP_OFF = new Color(95, 95, 95);
        /// <summary>
        /// Color that represents the jump ice on block. 
        /// </summary>
        public static readonly Color JUMP_ICE_ON = new Color(31, 32, 31);
        /// <summary>
        /// Color that represents the jump ice off block. 
        /// </summary>
        public static readonly Color JUMP_ICE_OFF = new Color(95, 96, 95);
        /// <summary>
        /// Color that represents the jump snow on block. 
        /// </summary>
        public static readonly Color JUMP_SNOW_ON = new Color(31, 33, 31);
        /// <summary>
        /// Color that represents the jump snow off block. 
        /// </summary>
        public static readonly Color JUMP_SNOW_OFF = new Color(95, 97, 95);

        /// <summary>
        /// Color that represents the sand on block. 
        /// </summary>
        public static readonly Color SAND_ON = new Color(238, 124, 40);
        /// <summary>
        /// Color that represents the sand off block. 
        /// </summary>
        public static readonly Color SAND_OFF = new Color(40, 124, 238);
        /// <summary>
        /// Color that represents the sand lever block. 
        /// </summary>
        public static readonly Color SAND_LEVER = new Color(238, 41, 124);
        /// <summary>
        /// Color that represents the sand lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color SAND_LEVER_ON = new Color(238, 42, 124);
        /// <summary>
        /// Color that represents the sand lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color SAND_LEVER_OFF = new Color(238, 43, 124);
        /// <summary>
        /// Color that represents the sand solid lever block. 
        /// </summary>
        public static readonly Color SAND_LEVER_SOLID = new Color(238, 44, 124);
        /// <summary>
        /// Color that represents the sand solid lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color SAND_LEVER_SOLID_ON = new Color(238, 45, 124);
        /// <summary>
        /// Color that represents the sand solid lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color SAND_LEVER_SOLID_OFF = new Color(238, 46, 124);
    }
}
