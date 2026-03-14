namespace SwitchBlocks.Menus
{
    using System.IO;
    using System.Xml.Linq;
    using BehaviorTree;
    using JumpKing;
    using Setups;

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
                var elementAuto = AddElementOrComment(target, source, "Auto", isParent: true);
                var sourceAuto = source?.Element("Auto");

                AddElementOrComment(elementAuto, sourceAuto, "Duration", "3.0");
                AddElementOrComment(elementAuto, sourceAuto, "DurationOff", "3.0");
                AddElementOrComment(elementAuto, sourceAuto, "Multiplier", "1.0");
                AddElementOrComment(elementAuto, sourceAuto, "ForceStateSwitch");

                var warnElement = AddElementOrComment(elementAuto, sourceAuto, "Warn", isParent: true);
                var sourceWarn = sourceAuto?.Element("Warn");

                AddElementOrComment(warnElement, sourceWarn, "Count", "2");
                AddElementOrComment(warnElement, sourceWarn, "Duration", "1.0");
                AddElementOrComment(warnElement, sourceWarn, "DisableOn");
                AddElementOrComment(warnElement, sourceWarn, "DisableOff");
            }

            if (SetupBasic.IsUsed)
            {
                var elementBasic = AddElementOrComment(target, source, "Basic", isParent: true);
                var sourceBasic = source?.Element("Basic");

                AddElementOrComment(elementBasic, sourceBasic, "Multiplier", "1.0");
                AddElementOrComment(elementBasic, sourceBasic, "LeverSideDisable", "Up, Down, Left, Right");
            }

            if (SetupCountdown.IsUsed)
            {
                var elementCountdown = AddElementOrComment(target, source, "Countdown", isParent: true);
                var sourceCountdown = source?.Element("Countdown");

                AddElementOrComment(elementCountdown, sourceCountdown, "Duration", "3.0");
                AddElementOrComment(elementCountdown, sourceCountdown, "Multiplier", "1");
                AddElementOrComment(elementCountdown, sourceCountdown, "LeverSideDisable", "Up, Down, Left, Right");
                AddElementOrComment(elementCountdown, sourceCountdown, "ForceStateSwitch");
                AddElementOrComment(elementCountdown, sourceCountdown, "SingleUseReset");

                var warnElement = AddElementOrComment(elementCountdown, sourceCountdown, "Warn", isParent: true);
                var sourceWarn = sourceCountdown?.Element("Warn");

                AddElementOrComment(warnElement, sourceWarn, "Count", "2");
                AddElementOrComment(warnElement, sourceWarn, "Duration", "1.0");
            }

            if (SetupGroup.IsUsed)
            {
                var elementGroup = AddElementOrComment(target, source, "Group", isParent: true);
                var sourceGroup = source?.Element("Group");

                AddElementOrComment(elementGroup, sourceGroup, "Duration", "0");
                AddElementOrComment(elementGroup, sourceGroup, "Multiplier", "1.0");
                AddElementOrComment(elementGroup, sourceGroup, "LeverSideDisable", "Up, Down, Left, Right");
                AddElementOrComment(elementGroup, sourceGroup, "PlatformSideDisable", "Up, Down, Left, Right");
            }

            if (SetupJump.IsUsed)
            {
                var elementJump = AddElementOrComment(target, source, "Jump", isParent: true);
                var sourceJump = source?.Element("Jump");

                AddElementOrComment(elementJump, sourceJump, "Multiplier", "1.0");
                AddElementOrComment(elementJump, sourceJump, "ForceStateSwitch");
                // TODO: Uncomment after Cloudy releases his map.
                //AddElementOrComment(elementJump, sourceJump, "CanJumpInAir", "false");
                //AddElementOrComment(elementJump, sourceJump, "Cooldown", "0");
            }

            if (SetupSand.IsUsed)
            {
                var elementSand = AddElementOrComment(target, source, "Sand", isParent: true);
                var sourceSand = source?.Element("Sand");

                // v2 is disabled for now.
                //AddElementOrComment(elementSand, sourceSand, "IsV2", "false");
                AddElementOrComment(elementSand, sourceSand, "Multiplier", "1.0");
                AddElementOrComment(elementSand, sourceSand, "LeverSideDisable", "Up, Down, Left, Right");
            }

            // ReSharper disable once InvertIf
            if (SetupSequence.IsUsed)
            {
                var elementSequence = AddElementOrComment(target, source, "Sequence", isParent: true);
                var sourceSequence = source?.Element("Sequence");

                AddElementOrComment(elementSequence, sourceSequence, "Duration", "0.0");
                AddElementOrComment(elementSequence, sourceSequence, "Multiplier", "1.0");
                AddElementOrComment(elementSequence, sourceSequence, "LeverSideDisable", "Up, Down, Left, Right");
                AddElementOrComment(elementSequence, sourceSequence, "PlatformSideDisable", "Up, Down, Left, Right");
                AddElementOrComment(elementSequence, sourceSequence, "DisableOnLeaving");
                AddElementOrComment(elementSequence, sourceSequence, "DefaultActive", "1");
            }

            newDoc.Save(file);
            newDoc.Save(Path.Combine(directoryMod, "blocks.xml"));

            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }

        /// <summary>
        ///     Adds either an <see cref="XElement" /> or <see cref="XComment" /> to the target depending on if
        ///     the source contained the asked for element and if that element is a parent to other elements or
        ///     a "leaf" element.
        /// </summary>
        /// <param name="targetParent">The XElement to add to.</param>
        /// <param name="sourceParent">The XElement to take from.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="defaultValue">Default value should a comment be added.</param>
        /// <param name="isParent">If the source is parent to others or a leaf.</param>
        /// <returns><see cref="XElement" /> for further adding to, or <c>null</c> if the element is a leaf.</returns>
        private static XElement AddElementOrComment(
            XElement targetParent,
            XElement sourceParent,
            string elementName,
            string defaultValue = null,
            bool isParent = false)
        {
            if (isParent)
            {
                var parentElement = new XElement(elementName);
                targetParent.Add(parentElement);
                return parentElement;
            }

            var element = sourceParent?.Element(elementName);
            if (element != null)
            {
                targetParent.Add(element);
                return null;
            }

            targetParent.Add(defaultValue == null
                ? new XComment($" <{elementName} /> ")
                : new XComment($" <{elementName}>{defaultValue}</{elementName}> "));

            return null;
        }
    }
}
