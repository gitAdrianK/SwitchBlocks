namespace SwitchBlocks.Factories
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using JumpKing;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Util;

    public class FactoryDrawables
    {
        // Don't look :D
        // Calling it a factory makes it okay to have infinite method parameters, right.

        public enum DrawType
        {
            Platforms,
            Levers,
        }

        public enum BlockType
        {
            Auto,
            Basic,
            Countdown,
            Jump,
            Sand,
        }

        /// <summary>
        /// Creates all drawbles for a given DrawType and BlockType.
        /// Entities are added to the manager automatically, this will not return anything.
        /// </summary>
        /// <param name="drawType">If the drawable is a lever or a platform</param>
        /// <param name="blockType">What type of block should be created</param>
        public static void CreateDrawables<T>(DrawType drawType, BlockType blockType, EntityLogic<T> entityLogic) where T : IDataProvider
        {
            var contentManager = Game1.instance.contentManager;
            var sep = Path.DirectorySeparatorChar;
            var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}{drawType.ToString().ToLower()}{sep}{blockType}{sep}";

            if (!Directory.Exists(path))
            {
                return;
            }

            var files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return;
            }

            var regex = GetRegex(drawType);

            foreach (var file in files)
            {
                var fileName = file.Split(sep).Last();

                if (!regex.IsMatch(fileName))
                {
                    continue;
                }

                var screen = int.Parse(Regex.Replace(fileName, @"[^\d]", "")) - 1;
                var document = new XmlDocument();
                document.Load(file);
                GetDrawables(document.LastChild.ChildNodes, drawType, blockType, entityLogic, screen, path, sep);
            }
        }

        private static Regex GetRegex(DrawType drawType)
        {
            // It seems "Verbatim text" and "String interpolation" together are C# 6 and up.
            // So a simple
            // new Regex(@$"^{drawType.ToString().ToLower()}(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
            // would be cool, but whatever.
            switch (drawType)
            {

                case DrawType.Platforms:
                    return new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
                case DrawType.Levers:
                    return new Regex(@"^levers(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
                default:
                    throw new NotImplementedException("Unknown Draw Type, cannot create regex!");
            }
        }

        private static void GetDrawables<T>(
            XmlNodeList list,
            DrawType drawType,
            BlockType blockType,
            EntityLogic<T> entityLogic,
            int screen,
            string path,
            char sep)
            where T : IDataProvider
        {
            foreach (XmlElement element in list)
            {
                var drawable = element.ChildNodes;
                switch (drawType)
                {
                    case DrawType.Platforms:
                        GetPlatforms(drawable, blockType, entityLogic, screen, path, sep);
                        break;
                    case DrawType.Levers:
                        GetLevers(drawable, blockType, screen, path, sep);
                        break;
                    default:
                        throw new NotImplementedException("Unknown Draw Type, cannot create entity!");
                }
            }
        }

        private static void GetLevers(
            XmlNodeList drawable,
            BlockType blockType,
            int screen,
            string path,
            char sep)
        {
            var dictionary = Xml.MapNamesRequired(drawable,
                ModStrings.TEXTURE,
                ModStrings.POSITION);
            if (dictionary == null)
            {
                return;
            }

            var position = Xml.GetVector2(drawable[dictionary[ModStrings.POSITION]]);
            if (!position.HasValue)
            {
                return;
            }

            var filePath = $"{path}{ModStrings.TEXTURES}{sep}{drawable[dictionary[ModStrings.TEXTURE]].InnerText}";
            if (!File.Exists($"{filePath}.xnb"))
            {
                return;
            }
            var texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");

            switch (blockType)
            {
                case BlockType.Auto:
                case BlockType.Jump:
                    // These types do not have levers
                    break;
                case BlockType.Basic:
                    _ = new EntityDrawLever(texture, position.Value, screen, DataBasic.Instance);
                    break;
                case BlockType.Countdown:
                    _ = new EntityDrawLever(texture, position.Value, screen, DataCountdown.Instance);
                    break;
                case BlockType.Sand:
                    _ = new EntityDrawLever(texture, position.Value, screen, DataSand.Instance);
                    break;
                default:
                    throw new NotImplementedException("Unknown Block Type, cannot create entity!");
            }
        }

        private static void GetPlatforms<T>(
            XmlNodeList drawable,
            BlockType blockType,
            EntityLogic<T> entityLogic,
            int screen,
            string path,
            char sep)
            where T : IDataProvider
        {
            switch (blockType)
            {
                case BlockType.Auto:
                    GetPlatform(
                        drawable,
                        DataAuto.Instance,
                        entityLogic,
                        screen,
                        path,
                        sep);
                    break;
                case BlockType.Basic:
                    GetPlatform(
                        drawable,
                        DataBasic.Instance,
                        entityLogic,
                        screen,
                        path,
                        sep);
                    break;
                case BlockType.Countdown:
                    GetPlatform(
                        drawable,
                        DataCountdown.Instance,
                        entityLogic,
                        screen,
                        path,
                        sep);
                    break;
                case BlockType.Jump:
                    GetPlatform(
                        drawable,
                        DataJump.Instance,
                        entityLogic,
                        screen,
                        path,
                        sep);
                    break;
                case BlockType.Sand:
                    GetPlatformSand(
                        drawable,
                        DataSand.Instance,
                        screen,
                        path,
                        sep);
                    break;
                default:
                    throw new NotImplementedException("Unknown Block Type, cannot create entity!");
            }
        }

        private static void GetPlatform<T>(
            XmlNodeList drawable,
            IDataProvider dataProvider,
            EntityLogic<T> entityLogic,
            int screen,
            string path,
            char sep)
            where T : IDataProvider
        {
            var dictionary = Xml.MapNamesRequired(drawable,
                ModStrings.TEXTURE,
                ModStrings.POSITION,
                ModStrings.START_STATE);
            if (dictionary == null)
            {
                return;
            }

            var position = Xml.GetVector2(drawable[dictionary[ModStrings.POSITION]]);
            if (!position.HasValue)
            {
                return;
            }

            var filePath = $"{path}{ModStrings.TEXTURES}{sep}{drawable[dictionary[ModStrings.TEXTURE]].InnerText}";
            if (!File.Exists($"{filePath}.xnb"))
            {
                return;
            }
            var texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");

            var startState = drawable[dictionary[ModStrings.START_STATE]].InnerText.ToLower() == "on";

            Animation animation;
            if (dictionary.TryGetValue(ModStrings.ANIMATION, out var index))
            {
                animation = Xml.GetAnimation(drawable[index]);
            }
            else
            {
                animation = default;
            }

            Animation animationOut;
            if (dictionary.TryGetValue(ModStrings.ANIMATION_OUT, out index))
            {
                animationOut = Xml.GetAnimation(drawable[index]);
            }
            else
            {
                animationOut = animation;
            }

            if (dictionary.TryGetValue(ModStrings.SPRITES, out index))
            {
                var children = drawable[index].ChildNodes;
                var dictSprites = Xml.MapNamesRequired(children,
                    ModStrings.CELLS);
                if (dictSprites == null)
                {
                    return;
                }

                var cells = Xml.GetPoint(children[dictSprites[ModStrings.CELLS]]);
                if (!cells.HasValue)
                {
                    return;
                }

                float[] frames = null;
                if (dictSprites.TryGetValue(ModStrings.FRAMES, out index))
                {
                    var numbers = children[index].ChildNodes;
                    frames = new float[numbers.Count];
                    for (var i = 0; i < numbers.Count; i++)
                    {
                        frames[i] = float.Parse(numbers[i].InnerText, CultureInfo.InvariantCulture);
                    }
                }

                var fps = 1.0f;
                if (dictSprites.TryGetValue(ModStrings.FPS, out index))
                {
                    fps = float.Parse(children[index].InnerText, CultureInfo.InvariantCulture);
                }

                var randomOffset = dictSprites.ContainsKey(ModStrings.OFFSET);

                entityLogic.AddScreen(screen);

                _ = new EntityDrawPlatformLoop(
                    texture,
                    position.Value,
                    startState,
                    animation,
                    animationOut,
                    screen,
                    dataProvider,
                    cells.Value,
                    fps,
                    frames,
                    randomOffset);
            }
            else
            {
                entityLogic.AddScreen(screen);

                _ = new EntityDrawPlatform(
                    texture,
                    position.Value,
                    startState,
                    animation,
                    animationOut,
                    screen,
                    dataProvider);
            }
        }

        private static void GetPlatformSand(
            XmlNodeList drawable,
            IDataProvider dataProvider,
            int screen,
            string path,
            char sep)
        {
            var dictionary = Xml.MapNamesRequired(drawable,
                ModStrings.POSITION,
                ModStrings.START_STATE);

            if (dictionary == null)
            {
                return;
            }

            string filePath;
            // Require at least one of the size giving textures to exist (Background or Foregroud)
            if (!dictionary.ContainsKey(ModStrings.BACKGROUND) && !dictionary.ContainsKey(ModStrings.FOREGROUND))
            {
                return;
            }

            var contentManager = Game1.instance.contentManager;
            // Background
            Texture2D background = null;
            if (dictionary.ContainsKey(ModStrings.BACKGROUND))
            {
                filePath = $"{path}{ModStrings.TEXTURES}{sep}{drawable[dictionary[ModStrings.BACKGROUND]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    return;
                }
                background = contentManager.Load<Texture2D>($"{filePath}");
            }

            // Scrolling
            Texture2D scrolling = null;
            if (dictionary.ContainsKey(ModStrings.SCROLLING))
            {
                filePath = $"{path}{ModStrings.TEXTURES}{sep}{drawable[dictionary[ModStrings.SCROLLING]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    return;
                }
                scrolling = contentManager.Load<Texture2D>($"{filePath}");
            }

            // Foreground
            Texture2D foreground = null;
            if (dictionary.ContainsKey(ModStrings.FOREGROUND))
            {
                filePath = $"{path}{ModStrings.TEXTURES}{sep}{drawable[dictionary[ModStrings.FOREGROUND]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    return;
                }
                foreground = contentManager.Load<Texture2D>($"{filePath}");
            }

            // Make sure at least one texture existed so a size can be set.
            if ((background == null) && (foreground == null))
            {
                return;
            }

            // Position
            var position = Xml.GetVector2(drawable[dictionary[ModStrings.POSITION]]);
            if (!position.HasValue)
            {
                return;
            }

            var startState = drawable[dictionary[ModStrings.START_STATE]].InnerText.ToLower() == "on";

            _ = new EntityDrawPlatformSand(
                background,
                scrolling,
                foreground,
                position.Value,
                startState,
                screen,
                dataProvider);
        }
    }
}
