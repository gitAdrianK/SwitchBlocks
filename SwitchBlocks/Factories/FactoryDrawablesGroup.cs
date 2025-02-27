namespace SwitchBlocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using JumpKing;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Setups;
    using SwitchBlocks.Util;

    public class FactoryDrawablesGroup
    {
        // There are no levers for both group types

        public enum BlockType
        {
            Group,
            Sequence,
        }

        /// <summary>
        /// Creates all drawbles for a given BlockType.
        /// Entities are added to the manager automatically, this will not return anything.
        /// </summary>
        /// <param name="blockType">What type of block should be created</param>
        public static void CreateDrawables<T>(BlockType blockType, EntityGroupLogic<T> entityGroupLogic) where T : IGroupDataProvider
        {
            {
                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}platforms{sep}{blockType}{sep}";

                if (!Directory.Exists(path))
                {
                    return;
                }

                var files = Directory.GetFiles(path);
                if (files.Length == 0)
                {
                    return;
                }

                var regex = new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");

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
                    GetDrawables(document.LastChild.ChildNodes, blockType, entityGroupLogic, screen, path, sep);
                }
            }
        }

        private static void GetDrawables<T>(
            XmlNodeList list,
            BlockType blockType,
            EntityGroupLogic<T> entityGroupLogic,
            int screen,
            string path,
            char sep)
            where T : IGroupDataProvider
        {
            foreach (XmlElement element in list)
            {
                var drawable = element.ChildNodes;
                GetPlatforms(drawable, blockType, entityGroupLogic, screen, path, sep);
            }
        }

        private static void GetPlatforms<T>(
            XmlNodeList drawable,
            BlockType blockType,
            EntityGroupLogic<T> entityGroupLogic,
            int screen,
            string path,
            char sep)
            where T : IGroupDataProvider
        {
            switch (blockType)
            {
                case BlockType.Group:
                    GetPlatformGroup(
                        drawable,
                        DataGroup.Instance,
                        entityGroupLogic,
                        screen,
                        path,
                        sep,
                        SetupGroup.BlocksGroupA,
                        SetupGroup.BlocksGroupB,
                        SetupGroup.BlocksGroupC,
                        SetupGroup.BlocksGroupD);
                    break;
                case BlockType.Sequence:
                    GetPlatformGroup(
                        drawable,
                        DataSequence.Instance,
                        entityGroupLogic,
                        screen,
                        path,
                        sep,
                        SetupSequence.BlocksSequenceA,
                        SetupSequence.BlocksSequenceB,
                        SetupSequence.BlocksSequenceC,
                        SetupSequence.BlocksSequenceD);
                    break;
                default:
                    throw new NotImplementedException("Unknown Block Type, cannot create entity!");
            }
        }

        private static void GetPlatformGroup<T>(
            XmlNodeList drawable,
            IGroupDataProvider dataProvider,
            EntityGroupLogic<T> entityGroupLogic,
            int screen,
            string path,
            char sep,
            params Dictionary<int, IBlockGroupId>[] blockGroups)
            where T : IGroupDataProvider
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

            var link = ((screen + 1) * 10000) + ((int)(position.Value.X / 8) * 100) + (int)(position.Value.Y / 8);
            if (dictionary.ContainsKey(ModStrings.LINK_POSITION))
            {
                var optionalLink = Xml.GetLink(drawable[dictionary[ModStrings.LINK_POSITION]]);
                if (optionalLink == null)
                {
                    return;
                }
                link = (int)optionalLink;
            }

            int groupId;
            foreach (var blockGroup in blockGroups)
            {
                if (blockGroup.ContainsKey(link))
                {
                    groupId = blockGroup[link].GroupId;
                    goto Found;
                }
            }
            return;
            Found:

            entityGroupLogic.AddScreen(screen);

            _ = new EntityDrawPlatformGroup(
                texture,
                position.Value,
                false,
                animation,
                animationOut,
                screen,
                groupId,
                dataProvider);
        }
    }
}
