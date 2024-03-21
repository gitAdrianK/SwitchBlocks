using JumpKing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SwitchBlocksMod.Util
{
    /// <summary>
    /// Contains colors used in this mod for its blocks.<br />
    /// Initializes the blocks color from "Steam Workshop Path\1061090\MAP ID\SwitchBlocksMod\blocks.xml"<br />
    /// As a color can be null one should ask if the blocks associated with the type
    /// of block (auto, basic, countdown, sand) are initialized.<br />
    /// The boolean value is only true should all blocks of a type be initialized. 
    /// </summary>
    public static class ModBlocks
    {
        private static char sep = Path.DirectorySeparatorChar;
        private static string path = $"{Game1.instance.contentManager.root}{sep}switchBlocksMod{sep}blocks.xml";

        /// <summary>
        /// If the auto blocks have been functionally initialized and are ready to be used.<br />
        /// A functional initialization is at least one platform, and a duration greater zero.
        /// </summary>
        public static readonly bool IS_AUTO_FUNCTIONALLY_INITIALIZED;
        /// <summary>
        /// Color that represents the auto on block. 
        /// </summary>
        public static readonly Color? AUTO_ON;
        /// <summary>
        /// Color that represents the auto off block. 
        /// </summary>
        public static readonly Color? AUTO_OFF;
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static readonly float AUTO_DURATION;

        /// <summary>
        /// If the basic blocks have been initialized and are ready to be used.<br />
        /// A functional initialization is at least one platform, and either a lever, or both On/Off levers.
        /// </summary>
        public static readonly bool IS_BASIC_FUNCTIONALLY_INITIALIZED;
        /// <summary>
        /// Color that represents the basic on block. 
        /// </summary>
        public static readonly Color? BASIC_ON;
        /// <summary>
        /// Color that represents the basic off block. 
        /// </summary>
        public static readonly Color? BASIC_OFF;
        /// <summary>
        /// Color that represents the basic solid lever block. 
        /// </summary>
        public static readonly Color? BASIC_LEVER;
        /// <summary>
        /// Color that represents the basic lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color? BASIC_LEVER_ON;
        /// <summary>
        /// Color that represents the basic lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color? BASIC_LEVER_OFF;
        /// <summary>
        /// Color that represents the basic lever block. 
        /// </summary>
        public static readonly Color? BASIC_LEVER_SOLID;
        /// <summary>
        /// Color that represents the basic solid lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color? BASIC_LEVER_SOLID_ON;
        /// <summary>
        /// Color that represents the basic solid lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color? BASIC_LEVER_SOLID_OFF;

        /// <summary>
        /// If the countdown blocks have been initialized and are ready to be used.<br />
        /// A functional initialization is at least one platform, a lever, and a duration greater zero.
        /// </summary>
        public static readonly bool IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED;
        /// <summary>
        /// Color that represents the countdown on block. 
        /// </summary>
        public static readonly Color? COUNTDOWN_ON;
        /// <summary>
        /// Color that represents the countdown off block. 
        /// </summary>
        public static readonly Color? COUNTDOWN_OFF;
        /// <summary>
        /// Color that represents the countdown lever block. 
        /// </summary>
        public static readonly Color? COUNTDOWN_LEVER;
        /// <summary>
        /// Color that represents the countdown solid lever block. 
        /// </summary>
        public static readonly Color? COUNTDOWN_LEVER_SOLID;
        /// <summary>
        /// How long the blocks stay in their state before switching.
        /// </summary>
        public static readonly float COUNTDOWN_DURATION;

        /// <summary>
        /// If the sand blocks have been initialized and are ready to be used.<br />
        /// A functional initialization is at least one platform, and either a lever, or both On/Off levers.
        /// </summary>
        public static readonly bool IS_SAND_FUNCTIONALLY_INITIALIZED;
        /// <summary>
        /// Color that represents the sand on block. 
        /// </summary>
        public static readonly Color? SAND_ON;
        /// <summary>
        /// Color that represents the sand off block. 
        /// </summary>
        public static readonly Color? SAND_OFF;
        /// <summary>
        /// Color that represents the sand lever block. 
        /// </summary>
        public static readonly Color? SAND_LEVER;
        /// <summary>
        /// Color that represents the sand lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color? SAND_LEVER_ON;
        /// <summary>
        /// Color that represents the sand lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color? SAND_LEVER_OFF;
        /// <summary>
        /// Color that represents the sand solid lever block. 
        /// </summary>
        public static readonly Color? SAND_LEVER_SOLID;
        /// <summary>
        /// Color that represents the sand solid lever block, that can only turn the state on. 
        /// </summary>
        public static readonly Color? SAND_LEVER_SOLID_ON;
        /// <summary>
        /// Color that represents the sand solid lever block, that can only turn the state off. 
        /// </summary>
        public static readonly Color? SAND_LEVER_SOLID_OFF;

        // Hurray for readonly!

        /// <summary>
        /// Static ctor.
        /// </summary>
        static ModBlocks()
        {
            if (!File.Exists(path))
            {
                return;
            }
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNode blocks = document.LastChild;
            if (blocks.Name != "Blocks")
            {
                return;
            }
            foreach (XmlNode block in blocks)
            {
                switch (block.Name)
                {
                    case "Auto":
                        ValueTuple<Color?, Color?>? autoPlatformsTuple = LoadPlatforms(block);
                        if (autoPlatformsTuple == null)
                        {
                            break;
                        }
                        (Color?, Color?) autoPlatformsValues = autoPlatformsTuple.Value;
                        AUTO_ON = autoPlatformsValues.Item1;
                        AUTO_OFF = autoPlatformsValues.Item2;
                        AUTO_DURATION = LoadDuration(block);
                        bool isAtLeastOneAutoPlatform = AUTO_ON != null || AUTO_OFF != null;
                        IS_AUTO_FUNCTIONALLY_INITIALIZED = (isAtLeastOneAutoPlatform && AUTO_DURATION > 0.0f);
                        break;

                    case "Basic":
                        ValueTuple<Color?, Color?>? basicPlatformsTuple = LoadPlatforms(block);
                        ValueTuple<Color?, Color?, Color?, Color?, Color?, Color?>? basicLeversTuple = LoadLevers(block);
                        if (basicPlatformsTuple == null || basicLeversTuple == null)
                        {
                            break;
                        }
                        (Color?, Color?) basicPlatformsValues = basicPlatformsTuple.Value;
                        (Color?, Color?, Color?, Color?, Color?, Color?) basicLeversValues = basicLeversTuple.Value;
                        BASIC_ON = basicPlatformsValues.Item1;
                        BASIC_OFF = basicPlatformsValues.Item2;
                        BASIC_LEVER = basicLeversValues.Item1;
                        BASIC_LEVER_ON = basicLeversValues.Item2;
                        BASIC_LEVER_OFF = basicLeversValues.Item3;
                        BASIC_LEVER_SOLID = basicLeversValues.Item4;
                        BASIC_LEVER_SOLID_ON = basicLeversValues.Item5;
                        BASIC_LEVER_SOLID_OFF = basicLeversValues.Item6;
                        bool isAtLeastOneBasicPlatform = BASIC_ON != null || BASIC_OFF != null;
                        bool isAtLeastOneBasicLever = BASIC_LEVER != null || BASIC_LEVER_SOLID != null;
                        bool areOneOfEachBasicLeverOnOff = (BASIC_LEVER_ON != null || BASIC_LEVER_SOLID_ON != null)
                            && (BASIC_LEVER_OFF != null || BASIC_LEVER_SOLID_OFF != null);
                        IS_BASIC_FUNCTIONALLY_INITIALIZED = isAtLeastOneBasicPlatform && (isAtLeastOneBasicLever || areOneOfEachBasicLeverOnOff);
                        break;

                    case "Countdown":
                        ValueTuple<Color?, Color?>? countdownPlatformsTuple = LoadPlatforms(block);
                        ValueTuple<Color?, Color?, Color?, Color?, Color?, Color?>? countdownLeversTuple = LoadLevers(block);
                        if (countdownPlatformsTuple == null || countdownLeversTuple == null)
                        {
                            break;
                        }
                        (Color?, Color?) countdownPlatformsValues = countdownPlatformsTuple.Value;
                        (Color?, Color?, Color?, Color?, Color?, Color?) countdownLeversValues = countdownLeversTuple.Value;
                        COUNTDOWN_ON = countdownPlatformsValues.Item1;
                        COUNTDOWN_OFF = countdownPlatformsValues.Item2;
                        COUNTDOWN_LEVER = countdownLeversValues.Item1;
                        COUNTDOWN_LEVER_SOLID = countdownLeversValues.Item4;
                        COUNTDOWN_DURATION = LoadDuration(block);
                        bool isAtLeastOneCountdownPlatform = COUNTDOWN_ON != null || COUNTDOWN_OFF != null;
                        bool isAtLeastOneCountdownLever = COUNTDOWN_LEVER != null || COUNTDOWN_LEVER_SOLID != null;
                        IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED = isAtLeastOneCountdownPlatform && isAtLeastOneCountdownLever && COUNTDOWN_DURATION > 0.0f;
                        break;

                    case "Sand":
                        ValueTuple<Color?, Color?>? sandPlatformsTuple = LoadPlatforms(block);
                        ValueTuple<Color?, Color?, Color?, Color?, Color?, Color?>? sandLeversTuple = LoadLevers(block);
                        if (sandPlatformsTuple == null || sandLeversTuple == null)
                        {
                            break;
                        }
                        (Color?, Color?) sandPlatformsValues = sandPlatformsTuple.Value;
                        (Color?, Color?, Color?, Color?, Color?, Color?) sandLeversValues = sandLeversTuple.Value;
                        SAND_ON = sandPlatformsValues.Item1;
                        SAND_OFF = sandPlatformsValues.Item2;
                        SAND_LEVER = sandLeversValues.Item1;
                        SAND_LEVER_ON = sandLeversValues.Item2;
                        SAND_LEVER_OFF = sandLeversValues.Item3;
                        SAND_LEVER_SOLID = sandLeversValues.Item4;
                        SAND_LEVER_SOLID_ON = sandLeversValues.Item5;
                        SAND_LEVER_SOLID_OFF = sandLeversValues.Item6;
                        bool isAtLeastOneSandPlatform = SAND_ON != null || SAND_OFF != null;
                        bool isAtLeastOneSandLever = SAND_LEVER != null || SAND_LEVER_SOLID != null;
                        bool areOneOfEachSandLeverOnOff = (SAND_LEVER_ON != null || SAND_LEVER_SOLID_ON != null)
                            && (SAND_LEVER_OFF != null || SAND_LEVER_SOLID_OFF != null);
                        IS_SAND_FUNCTIONALLY_INITIALIZED = isAtLeastOneSandPlatform && (isAtLeastOneSandLever || areOneOfEachSandLeverOnOff);
                        break;

                    default:
                        // Do nothing.
                        break;
                }
            }

            // Looks for a "Duration" node and returns the inside declared float duration or 3.0f.
            float LoadDuration(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionary = Xml.MapNames(children, "Duration");
                float duration = 3.0f;
                if (dictionary.ContainsKey("Duration"))
                {
                    duration = float.Parse(children[dictionary["Duration"]].InnerText);
                }
                return duration;
            }

            // Looks for "On" and "Off" nodes, the first value of the tuple being "On".
            (Color?, Color?)? LoadPlatforms(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionary = Xml.MapNames(children, "On", "Off");
                Color? on = null;
                if (dictionary.ContainsKey("On"))
                {
                    on = Xml.GetColor(children[dictionary["On"]]);
                }
                Color? off = null;
                if (dictionary.ContainsKey("Off"))
                {
                    off = Xml.GetColor(children[dictionary["Off"]]);
                }
                return (on, off);
            }

            // Looks for the different types of lever
            // Lever -> LeverOn -> LeverOff -> LeverSolid -> LeverSolidOn -> LeverSolidOff
            (Color?, Color?, Color?, Color?, Color?, Color?)? LoadLevers(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionary = Xml.MapNames(
                    children,
                    "Lever",
                    "LeverOn",
                    "LeverOff",
                    "LeverSolid",
                    "LeverSolidOn",
                    "LeverSolidOff");
                Color? lever = null;
                if (dictionary.ContainsKey("Lever"))
                {
                    lever = Xml.GetColor(children[dictionary["Lever"]]);
                }
                Color? leverOn = null;
                if (dictionary.ContainsKey("LeverOn"))
                {
                    leverOn = Xml.GetColor(children[dictionary["LeverOn"]]);
                }
                Color? leverOff = null;
                if (dictionary.ContainsKey("LeverOff"))
                {
                    leverOff = Xml.GetColor(children[dictionary["LeverOff"]]);
                }
                Color? leverSolid = null;
                if (dictionary.ContainsKey("LeverSolid"))
                {
                    leverSolid = Xml.GetColor(children[dictionary["LeverSolid"]]);
                }
                Color? leverSolidOn = null;
                if (dictionary.ContainsKey("LeverSolidOn"))
                {
                    leverSolidOn = Xml.GetColor(children[dictionary["LeverSolidOn"]]);
                }
                Color? leverSolidOff = null;
                if (dictionary.ContainsKey("LeverSolidOff"))
                {
                    leverSolidOff = Xml.GetColor(children[dictionary["LeverSolidOff"]]);
                }
                return (lever,
                    leverOn,
                    leverOff,
                    leverSolid,
                    leverSolidOn,
                    leverSolidOff);
            }
        }
    }
}
