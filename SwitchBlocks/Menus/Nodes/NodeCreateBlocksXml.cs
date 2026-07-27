namespace SwitchBlocks.Menus
{
    using System.IO;
    using System.Xml.Linq;
    using BehaviorTree;
    using JumpKing;
    using Patches;
    using Setups;
    using Util;
    using static Util.XmlHelper.AddAs;

    /// <summary>
    ///     A <see cref="IBTnode" /> responsible for creating the blocks.xml.
    /// </summary>
    public class NodeCreateBlocksXml : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating blocks.xml.");

            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The path '{directoryBin}' did not follow Worldsmith structure.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryMod = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder);
            Directory.CreateDirectory(directoryMod);

            var file = Path.Combine(
                Game1.instance.contentManager.root,
                ModConstants.Folder,
                "blocks.xml");

            var doc = File.Exists(file)
                ? XDocument.Load(file)
                : new XDocument(new XElement("Blocks"));
            var newDoc = new XDocument(new XElement("Blocks"));

            var source = doc.Root;
            var target = newDoc.Root;

            this.CreateAutoSettings(target, source);
            this.CreateBasicSettings(target, source);
            this.CreateCountdownSettings(target, source);
            this.CreateGroupSettings(target, source);
            this.CreateJumpSettings(target, source);
            this.CreateSandSettings(target, source);
            this.CreateSequenceSettings(target, source);
            this.CreateThresholdSettings(target, source);

            newDoc.Save(file);
            newDoc.Save(Path.Combine(directoryMod, "blocks.xml"));

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished creating blocks.xml.");
            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }

        /// <summary>
        ///     Creates settings related to the auto block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateAutoSettings(XElement target, XElement source)
        {
            if (!SetupAuto.IsUsed)
            {
                return;
            }

            var elementAuto = XmlHelper.AddElementOrComment(target, source, "Auto", addAs: Parent);
            var sourceAuto = source?.Element("Auto");

            XmlHelper.AddElementOrComment(elementAuto, sourceAuto, "Duration", "3.0", Comment);
            XmlHelper.AddElementOrComment(elementAuto, sourceAuto, "DurationOff", "3.0", Comment);
            XmlHelper.AddElementOrComment(elementAuto, sourceAuto, "Multiplier", "1.0", Comment);
            XmlHelper.AddElementOrComment(elementAuto, sourceAuto, "ForceStateSwitch", addAs: Comment);

            var warnElement = XmlHelper.AddElementOrComment(elementAuto, sourceAuto, "Warn", addAs: Parent);
            var sourceWarn = sourceAuto?.Element("Warn");

            XmlHelper.AddElementOrComment(warnElement, sourceWarn, "Count", "2", Comment);
            XmlHelper.AddElementOrComment(warnElement, sourceWarn, "Duration", "1.0", Comment);
            XmlHelper.AddElementOrComment(warnElement, sourceWarn, "DisableOn", addAs: Comment);
            XmlHelper.AddElementOrComment(warnElement, sourceWarn, "DisableOff", addAs: Comment);
        }

        /// <summary>
        ///     Creates settings related to the basic block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateBasicSettings(XElement target, XElement source)
        {
            if (!SetupBasic.IsUsed)
            {
                return;
            }

            var elementBasic = XmlHelper.AddElementOrComment(target, source, "Basic", addAs: Parent);
            var sourceBasic = source?.Element("Basic");

            XmlHelper.AddElementOrComment(elementBasic, sourceBasic, "Multiplier", "1.0", Comment);
            XmlHelper.AddElementOrComment(elementBasic, sourceBasic, "LeverSideDisable", "Up, Down, Left, Right",
                Comment);
            XmlHelper.AddElementOrComment(elementBasic, sourceBasic, "SaveCarriesOver", addAs: Comment);
        }

        /// <summary>
        ///     Creates settings related to the countdown block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateCountdownSettings(XElement target, XElement source)
        {
            if (!SetupCountdown.IsUsed)
            {
                return;
            }

            var elementCountdown = XmlHelper.AddElementOrComment(target, source, "Countdown", addAs: Parent);
            var sourceCountdown = source?.Element("Countdown");

            XmlHelper.AddElementOrComment(elementCountdown, sourceCountdown, "Duration", "3.0", Comment);
            XmlHelper.AddElementOrComment(elementCountdown, sourceCountdown, "Multiplier", "1", Comment);
            XmlHelper.AddElementOrComment(elementCountdown, sourceCountdown, "LeverSideDisable",
                "Up, Down, Left, Right", Comment);
            XmlHelper.AddElementOrComment(elementCountdown, sourceCountdown, "ForceStateSwitch", addAs: Comment);
            XmlHelper.AddElementOrComment(elementCountdown, sourceCountdown, "SingleUseReset", addAs: Comment);

            var warnElement =
                XmlHelper.AddElementOrComment(elementCountdown, sourceCountdown, "Warn", addAs: Parent);
            var sourceWarn = sourceCountdown?.Element("Warn");

            XmlHelper.AddElementOrComment(warnElement, sourceWarn, "Count", "2", Comment);
            XmlHelper.AddElementOrComment(warnElement, sourceWarn, "Duration", "1.0", Comment);
        }

        /// <summary>
        ///     Creates settings related to the group block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateGroupSettings(XElement target, XElement source)
        {
            if (!SetupGroup.IsUsed)
            {
                return;
            }

            var elementGroup = XmlHelper.AddElementOrComment(target, source, "Group", addAs: Parent);
            var sourceGroup = source?.Element("Group");

            XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "Duration", "0", Comment);
            XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "Multiplier", "1.0", Comment);
            XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "LeverSideDisable", "Up, Down, Left, Right",
                Comment);
            XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "PlatformSideDisable",
                "Up, Down, Left, Right", Comment);
        }

        /// <summary>
        ///     Creates settings related to the jump block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateJumpSettings(XElement target, XElement source)
        {
            if (!SetupJump.IsUsed)
            {
                return;
            }

            var elementJump = XmlHelper.AddElementOrComment(target, source, "Jump", addAs: Parent);
            var sourceJump = source?.Element("Jump");

            XmlHelper.AddElementOrComment(elementJump, sourceJump, "Multiplier", "1.0", Comment);
            XmlHelper.AddElementOrComment(elementJump, sourceJump, "ForceStateSwitch", addAs: Comment);
            // TODO: Uncomment after Cloudy releases his map.
            //AddElementOrComment(elementJump, sourceJump, "CanJumpInAir", "false");
            //AddElementOrComment(elementJump, sourceJump, "Cooldown", "0");
        }

        /// <summary>
        ///     Creates settings related to the sand block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateSandSettings(XElement target, XElement source)
        {
            if (!SetupSand.IsUsed)
            {
                return;
            }

            var elementSand = XmlHelper.AddElementOrComment(target, source, "Sand", addAs: Parent);
            var sourceSand = source?.Element("Sand");

            XmlHelper.AddElementOrComment(elementSand, sourceSand, "IsV2", addAs: Comment);
            XmlHelper.AddElementOrComment(elementSand, sourceSand, "Multiplier", "1.0", Comment);
            XmlHelper.AddElementOrComment(elementSand, sourceSand, "LeverSideDisable", "Up, Down, Left, Right",
                Comment);
        }

        /// <summary>
        ///     Creates settings related to the sequence block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateSequenceSettings(XElement target, XElement source)
        {
            if (!SetupSequence.IsUsed)
            {
                return;
            }

            var elementSequence = XmlHelper.AddElementOrComment(target, source, "Sequence", addAs: Parent);
            var sourceSequence = source?.Element("Sequence");

            XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "Duration", "0.0", Comment);
            XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "Multiplier", "1.0", Comment);
            XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "LeverSideDisable",
                "Up, Down, Left, Right", Comment);
            XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "PlatformSideDisable",
                "Up, Down, Left, Right", Comment);
            XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "DisableOnLeaving", addAs: Comment);
            XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "DefaultActive", "1, 3, 5", Comment);
        }

        /// <summary>
        ///     Creates settings related to the threshold block type if it is used.
        ///     Takes existing settings from the source and add them to the target element.
        /// </summary>
        /// <param name="target"><see cref="XElement" /> to add settings to.</param>
        /// <param name="source"><see cref="XElement" /> to take existing settings from.</param>
        private void CreateThresholdSettings(XElement target, XElement source)
        {
            if (!SetupThreshold.IsUsed)
            {
                return;
            }

            var elementThreshold = XmlHelper.AddElementOrComment(target, source, "Threshold", addAs: Parent);
            var sourceThreshold = source?.Element("Threshold");

            elementThreshold.Add(new XComment(" Stats to check for are: Jumps, Falls, Time, Session "));
            XmlHelper.AddElementOrComment(elementThreshold, sourceThreshold, "Stat", "Falls", Comment);
            XmlHelper.AddElementOrComment(elementThreshold, sourceThreshold, "Count", "0", Comment);
            XmlHelper.AddElementOrComment(elementThreshold, sourceThreshold, "Multiplier", "1.0", Comment);
            XmlHelper.AddElementOrComment(elementThreshold, sourceThreshold, "ForceStateSwitch", addAs: Comment);
        }
    }
}
