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
                        ValueTuple<Color?, Color?, float>? autoTuple = LoadAuto(block);
                        if (autoTuple == null)
                        {
                            break;
                        }
                        (Color?, Color?, float) autoValues = autoTuple.Value;
                        AUTO_ON = autoValues.Item1;
                        AUTO_OFF = autoValues.Item2;
                        AUTO_DURATION = autoValues.Item3;
                        bool isAtLeastOneAutoPlatform = AUTO_ON != null || AUTO_OFF != null;
                        IS_AUTO_FUNCTIONALLY_INITIALIZED = (isAtLeastOneAutoPlatform && AUTO_DURATION > 0.0f);
                        break;

                    case "Basic":
                        ValueTuple<Color?, Color?>? basicPlatformsTuple = LoadBasicPlatforms(block);
                        ValueTuple<Color?, Color?, Color?, Color?, Color?, Color?>? basicLeversTuple = LoadBasicLevers(block);
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
                        ValueTuple<Color?, Color?, Color?, Color?, float>? countdownTuple = LoadCountdown(block);
                        if (countdownTuple == null)
                        {
                            break;
                        }
                        (Color?, Color?, Color?, Color?, float) countdownValues = countdownTuple.Value;
                        COUNTDOWN_ON = countdownValues.Item1;
                        COUNTDOWN_OFF = countdownValues.Item2;
                        COUNTDOWN_LEVER = countdownValues.Item3;
                        COUNTDOWN_LEVER_SOLID = countdownValues.Item4;
                        COUNTDOWN_DURATION = countdownValues.Item5;
                        bool isAtLeastOneCountdownPlatform = COUNTDOWN_ON != null || COUNTDOWN_OFF != null;
                        bool isAtLeastOneCountdownLever = COUNTDOWN_LEVER != null || COUNTDOWN_LEVER_SOLID != null;
                        IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED = isAtLeastOneCountdownPlatform && isAtLeastOneCountdownLever && COUNTDOWN_DURATION > 0.0f;
                        break;

                    case "Sand":
                        ValueTuple<Color?, Color?>? sandPlatformsTuple = LoadSandPlatforms(block);
                        ValueTuple<Color?, Color?, Color?, Color?, Color?, Color?>? sandLeversTuple = LoadSandLevers(block);
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

            (Color?, Color?, float)? LoadAuto(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionaryAuto = Xml.MapNames(children, "On", "Off", "Duration");
                Color? autoOn = null;
                if (dictionaryAuto.ContainsKey("On"))
                {
                    autoOn = Xml.GetColor(children[dictionaryAuto["On"]]);
                }
                Color? autoOff = null;
                if (dictionaryAuto.ContainsKey("Off"))
                {
                    autoOff = Xml.GetColor(children[dictionaryAuto["Off"]]);
                }
                float autoDuration = 3.0f;
                if (dictionaryAuto.ContainsKey("Duration"))
                {
                    autoDuration = float.Parse(children[dictionaryAuto["Duration"]].InnerText);
                }
                return (autoOn, autoOff, autoDuration);
            }

            (Color?, Color?)? LoadBasicPlatforms(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionaryBasic = Xml.MapNames(
                    children,
                    "On",
                    "Off");
                Color? basicOn = null;
                if (dictionaryBasic.ContainsKey("On"))
                {
                    basicOn = Xml.GetColor(children[dictionaryBasic["On"]]);
                }
                Color? basicOff = null;
                if (dictionaryBasic.ContainsKey("Off"))
                {
                    basicOff = Xml.GetColor(children[dictionaryBasic["Off"]]);
                }
                return (basicOn,
                    basicOff);
            }

            (Color?, Color?, Color?, Color?, Color?, Color?)? LoadBasicLevers(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionaryBasic = Xml.MapNames(
                    children,
                    "Lever",
                    "LeverOn",
                    "LeverOff",
                    "LeverSolid",
                    "LeverSolidOn",
                    "LeverSolidOff");
                Color? basicLever = null;
                if (dictionaryBasic.ContainsKey("Lever"))
                {
                    basicLever = Xml.GetColor(children[dictionaryBasic["Lever"]]);
                }
                Color? basicLeverOn = null;
                if (dictionaryBasic.ContainsKey("LeverOn"))
                {
                    basicLeverOn = Xml.GetColor(children[dictionaryBasic["LeverOn"]]);
                }
                Color? basicLeverOff = null;
                if (dictionaryBasic.ContainsKey("LeverOff"))
                {
                    basicLeverOff = Xml.GetColor(children[dictionaryBasic["LeverOff"]]);
                }
                Color? basicLeverSolid = null;
                if (dictionaryBasic.ContainsKey("LeverSolid"))
                {
                    basicLeverSolid = Xml.GetColor(children[dictionaryBasic["LeverSolid"]]);
                }
                Color? basicLeverSolidOn = null;
                if (dictionaryBasic.ContainsKey("LeverSolidOn"))
                {
                    basicLeverSolidOn = Xml.GetColor(children[dictionaryBasic["LeverSolidOn"]]);
                }
                Color? basicLeverSolidOff = null;
                if (dictionaryBasic.ContainsKey("LeverSolidOff"))
                {
                    basicLeverSolidOff = Xml.GetColor(children[dictionaryBasic["LeverSolidOff"]]);
                }
                return (basicLever,
                    basicLeverOn,
                    basicLeverOff,
                    basicLeverSolid,
                    basicLeverSolidOn,
                    basicLeverSolidOff);
            }

            (Color?, Color?, Color?, Color?, float)? LoadCountdown(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionaryCountdown = Xml.MapNames(
                    children,
                    "On",
                    "Off",
                    "Lever",
                    "LeverSolid",
                    "Duration");
                Color? countdownOn = null;
                if (dictionaryCountdown.ContainsKey("On"))
                {
                    countdownOn = Xml.GetColor(children[dictionaryCountdown["On"]]);
                }
                Color? countdownOff = null;
                if (dictionaryCountdown.ContainsKey("Off"))
                {
                    countdownOff = Xml.GetColor(children[dictionaryCountdown["Off"]]);
                }
                Color? countdownLever = null;
                if (dictionaryCountdown.ContainsKey("Lever"))
                {
                    countdownLever = Xml.GetColor(children[dictionaryCountdown["Lever"]]);
                }
                Color? countdownLeverSolid = null;
                if (dictionaryCountdown.ContainsKey("LeverSolid"))
                {
                    countdownLeverSolid = Xml.GetColor(children[dictionaryCountdown["LeverSolid"]]);
                }
                float countdownDuration = 3.0f;
                if (dictionaryCountdown.ContainsKey("Duration"))
                {
                    countdownDuration = float.Parse(children[dictionaryCountdown["Duration"]].InnerText);
                }
                return (countdownOn, countdownOff, countdownLever, countdownLeverSolid, countdownDuration);
            }

            (Color?, Color?)? LoadSandPlatforms(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionarySand = Xml.MapNames(
                    children,
                    "On",
                    "Off");
                Color? sandOn = null;
                if (dictionarySand.ContainsKey("On"))
                {
                    sandOn = Xml.GetColor(children[dictionarySand["On"]]);
                }
                Color? sandOff = null;
                if (dictionarySand.ContainsKey("Off"))
                {
                    sandOff = Xml.GetColor(children[dictionarySand["Off"]]);
                }
                return (sandOn,
                    sandOff);
            }

            (Color?, Color?, Color?, Color?, Color?, Color?)? LoadSandLevers(XmlNode root)
            {
                XmlNodeList children = root.ChildNodes;
                Dictionary<string, int> dictionarySand = Xml.MapNames(
                    children,
                    "Lever",
                    "LeverOn",
                    "LeverOff",
                    "LeverSolid",
                    "LeverSolidOn",
                    "LeverSolidOff");
                Color? sandLever = null;
                if (dictionarySand.ContainsKey("Lever"))
                {
                    sandLever = Xml.GetColor(children[dictionarySand["Lever"]]);
                }
                Color? sandLeverOn = null;
                if (dictionarySand.ContainsKey("LeverOn"))
                {
                    sandLeverOn = Xml.GetColor(children[dictionarySand["LeverOn"]]);
                }
                Color? sandLeverOff = null;
                if (dictionarySand.ContainsKey("LeverOff"))
                {
                    sandLeverOff = Xml.GetColor(children[dictionarySand["LeverOff"]]);
                }
                Color? sandLeverSolid = null;
                if (dictionarySand.ContainsKey("LeverSolid"))
                {
                    sandLeverSolid = Xml.GetColor(children[dictionarySand["LeverSolid"]]);
                }
                Color? sandLeverSolidOn = null;
                if (dictionarySand.ContainsKey("LeverSolidOn"))
                {
                    sandLeverSolidOn = Xml.GetColor(children[dictionarySand["LeverSolidOn"]]);
                }
                Color? sandLeverSolidOff = null;
                if (dictionarySand.ContainsKey("LeverSolidOff"))
                {
                    sandLeverSolidOff = Xml.GetColor(children[dictionarySand["LeverSolidOff"]]);
                }
                return (sandLever,
                    sandLeverOn,
                    sandLeverOff,
                    sandLeverSolid,
                    sandLeverSolidOn,
                    sandLeverSolidOff);
            }
        }
    }
}
