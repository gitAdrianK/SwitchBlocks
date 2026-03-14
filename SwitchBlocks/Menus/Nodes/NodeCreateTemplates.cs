namespace SwitchBlocks.Menus
{
    using System.IO;
    using System.Xml.Linq;
    using BehaviorTree;
    using JumpKing;
    using Util;
    using static Util.XmlHelper.AddAs;

    public class NodeCreateTemplates : IBTnode
    {
        protected override BTresult MyRun(TickData tickData)
        {
            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryTemplates = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder, "templates");
            Directory.CreateDirectory(directoryTemplates);

            this.CreateLevers(Path.Combine(directoryTemplates, "levers1.xml"));
            this.CreatePlatforms(Path.Combine(directoryTemplates, "platforms1.xml"));
            this.CreateScrolling(Path.Combine(directoryTemplates, "sands1.xml"), "Sands", "Sand");
            this.CreateScrolling(Path.Combine(directoryTemplates, "conveyors1.xml"), "Conveyors", "Conveyor");

            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }

        private void CreateLevers(string filePath)
        {
            var doc = new XDocument(new XElement("Levers"));
            var target = doc.Root;

            var lever = XmlHelper.AddElementOrComment(target, "Lever", addAs: Parent);
            this.CreateRequired(lever);
            XmlHelper.AddElementOrComment(lever, "IsForeground");

            doc.Save(filePath);
        }

        private void CreatePlatforms(string filePath)
        {
            var doc = new XDocument(new XElement("Platforms"));
            var target = doc.Root;

            target.Add(new XComment(" This is the minimum you need to create a platform "));
            var minPlatform = XmlHelper.AddElementOrComment(target, "Platform", addAs: Parent);
            this.CreateRequired(minPlatform);

            target.Add(new XComment(" These are all possible options "));
            var maxPlatform = XmlHelper.AddElementOrComment(target, "Platform", addAs: Parent);
            this.CreateRequired(maxPlatform);
            XmlHelper.AddElementOrComment(maxPlatform, "IsForeground");
            XmlHelper.AddElementOrComment(maxPlatform, "StartState", "On");
            this.CreateAnimation(maxPlatform, "Animation");
            this.CreateAnimation(maxPlatform, "AnimationOut");
            var sprites = XmlHelper.AddElementOrComment(maxPlatform, "Sprites", addAs: Parent);
            this.CreateSprites(sprites);

            doc.Save(filePath);
        }

        private void CreateScrolling(string filePath, string rootName, string elementName)
        {
            var doc = new XDocument(new XElement(rootName));
            var target = doc.Root;

            target.Add(new XComment(" This is the minimum you need to create a scrolling platform "));
            var minScrolling = XmlHelper.AddElementOrComment(target, elementName, addAs: Parent);
            minScrolling.Add(new XComment(" Background OR Foreground is required, the minimum is ONE of the two "));
            XmlHelper.AddElementOrComment(minScrolling, "Background", "TEXTURE NAME HERE");
            XmlHelper.AddElementOrComment(minScrolling, "Foreground", "TEXTURE NAME HERE");
            XmlHelper.AddElementOrComment(minScrolling, "Scrolling", "TEXTURE NAME HERE");
            var position = XmlHelper.AddElementOrComment(minScrolling, "Position", addAs: Parent);
            XmlHelper.AddElementOrComment(position, "X", "100");
            XmlHelper.AddElementOrComment(position, "Y", "50");

            target.Add(new XComment(" These are all possible options "));
            var maxPlatform = XmlHelper.AddElementOrComment(target, elementName, addAs: Parent);
            XmlHelper.AddElementOrComment(maxPlatform, "Background", "TEXTURE NAME HERE");
            XmlHelper.AddElementOrComment(maxPlatform, "Scrolling", "TEXTURE NAME HERE");
            XmlHelper.AddElementOrComment(maxPlatform, "Foreground", "TEXTURE NAME HERE");
            position = XmlHelper.AddElementOrComment(maxPlatform, "Position", addAs: Parent);
            XmlHelper.AddElementOrComment(position, "X", "100");
            XmlHelper.AddElementOrComment(position, "Y", "50");
            XmlHelper.AddElementOrComment(maxPlatform, "IsForeground");
            XmlHelper.AddElementOrComment(maxPlatform, "StartState", "On");
            XmlHelper.AddElementOrComment(maxPlatform, "Multiplier", "2.5");

            doc.Save(filePath);
        }

        private void CreateRequired(XElement drawableElement)
        {
            XmlHelper.AddElementOrComment(drawableElement, "Texture", "TEXTURE NAME HERE");
            var position = XmlHelper.AddElementOrComment(drawableElement, "Position", addAs: Parent);
            XmlHelper.AddElementOrComment(position, "X", "100");
            XmlHelper.AddElementOrComment(position, "Y", "50");
        }

        private void CreateAnimation(XElement parent, string name)
        {
            var animation = XmlHelper.AddElementOrComment(parent, name, addAs: Parent);
            XmlHelper.AddElementOrComment(animation, "Style", "Fade");
            XmlHelper.AddElementOrComment(animation, "Curve", "Linear");
        }

        private void CreateSprites(XElement parent)
        {
            var sprites = XmlHelper.AddElementOrComment(parent, "Sprites", addAs: Parent);
            var cells = XmlHelper.AddElementOrComment(sprites, "Cells", addAs: Parent);
            XmlHelper.AddElementOrComment(cells, "X", "2");
            XmlHelper.AddElementOrComment(cells, "Y", "2");
            sprites.Add(new XComment(" The Frames element overrides the FPS if defined "));
            XmlHelper.AddElementOrComment(sprites, "FPS", "3");
            var frames = XmlHelper.AddElementOrComment(sprites, "Frames", addAs: Parent);
            XmlHelper.AddElementOrComment(frames, "float", "1.2");
            XmlHelper.AddElementOrComment(frames, "float", "0.3");
            XmlHelper.AddElementOrComment(frames, "float", "0.3");
            XmlHelper.AddElementOrComment(sprites, "RandomOffset");
            XmlHelper.AddElementOrComment(sprites, "ResetWithLever");
            XmlHelper.AddElementOrComment(sprites, "IgnoreState");
        }
    }
}
