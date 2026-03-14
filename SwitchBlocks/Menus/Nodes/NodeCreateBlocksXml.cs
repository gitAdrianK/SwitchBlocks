namespace SwitchBlocks.Menus
{
    using System.IO;
    using System.Xml.Linq;
    using BehaviorTree;
    using JumpKing;
    using Setups;
    using Util;
    using static Util.XmlHelper.AddAs;

    /// <summary>
    ///     A BtNode responsible for creating the blocks.xml.
    /// </summary>
    public class NodeCreateBlocksXml : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
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

            // I'll get around to figuring a cleaner solution some time.
            if (SetupAuto.IsUsed)
            {
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

            if (SetupBasic.IsUsed)
            {
                var elementBasic = XmlHelper.AddElementOrComment(target, source, "Basic", addAs: Parent);
                var sourceBasic = source?.Element("Basic");

                XmlHelper.AddElementOrComment(elementBasic, sourceBasic, "Multiplier", "1.0", Comment);
                XmlHelper.AddElementOrComment(elementBasic, sourceBasic, "LeverSideDisable", "Up, Down, Left, Right",
                    Comment);
            }

            if (SetupCountdown.IsUsed)
            {
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

            if (SetupGroup.IsUsed)
            {
                var elementGroup = XmlHelper.AddElementOrComment(target, source, "Group", addAs: Parent);
                var sourceGroup = source?.Element("Group");

                XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "Duration", "0");
                XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "Multiplier", "1.0");
                XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "LeverSideDisable", "Up, Down, Left, Right");
                XmlHelper.AddElementOrComment(elementGroup, sourceGroup, "PlatformSideDisable",
                    "Up, Down, Left, Right");
            }

            if (SetupJump.IsUsed)
            {
                var elementJump = XmlHelper.AddElementOrComment(target, source, "Jump", addAs: Parent);
                var sourceJump = source?.Element("Jump");

                XmlHelper.AddElementOrComment(elementJump, sourceJump, "Multiplier", "1.0");
                XmlHelper.AddElementOrComment(elementJump, sourceJump, "ForceStateSwitch");
                // TODO: Uncomment after Cloudy releases his map.
                //AddElementOrComment(elementJump, sourceJump, "CanJumpInAir", "false");
                //AddElementOrComment(elementJump, sourceJump, "Cooldown", "0");
            }

            if (SetupSand.IsUsed)
            {
                var elementSand = XmlHelper.AddElementOrComment(target, source, "Sand", addAs: Parent);
                var sourceSand = source?.Element("Sand");

                // v2 is disabled for now.
                //AddElementOrComment(elementSand, sourceSand, "IsV2", "false");
                XmlHelper.AddElementOrComment(elementSand, sourceSand, "Multiplier", "1.0");
                XmlHelper.AddElementOrComment(elementSand, sourceSand, "LeverSideDisable", "Up, Down, Left, Right");
            }

            // ReSharper disable once InvertIf
            if (SetupSequence.IsUsed)
            {
                var elementSequence = XmlHelper.AddElementOrComment(target, source, "Sequence", addAs: Parent);
                var sourceSequence = source?.Element("Sequence");

                XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "Duration", "0.0");
                XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "Multiplier", "1.0");
                XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "LeverSideDisable",
                    "Up, Down, Left, Right");
                XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "PlatformSideDisable",
                    "Up, Down, Left, Right");
                XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "DisableOnLeaving");
                XmlHelper.AddElementOrComment(elementSequence, sourceSequence, "DefaultActive", "1");
            }

            newDoc.Save(file);
            newDoc.Save(Path.Combine(directoryMod, "blocks.xml"));

            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }
    }
}
