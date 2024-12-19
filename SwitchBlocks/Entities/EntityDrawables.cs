using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Patching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SwitchBlocks.Entities
{
    public abstract class EntityDrawables<TDrawable> : Entity where TDrawable : IDrawable
    {
        int currentScreen = -1;

        private readonly Dictionary<int, List<TDrawable>> drawablesDict;
        protected List<TDrawable> currentDrawables;
        public bool IsActiveOnCurrentScreen => currentDrawables != null;

        protected abstract void EntityUpdate(float p_delta);
        public abstract void EntityDraw(SpriteBatch spriteBatch);

        protected EntityDrawables(string xmlRootTag, string blocktype)
        {
            JKContentManager contentManager = Game1.instance.contentManager;

            // The root tag is with a capital and plural, the folder is all lower case,
            // the individual tag is singular.
            // e.g. folder: platforms, root tag: Platforms, individual tag: Platform
            string subfolder = xmlRootTag.ToLower();

            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}{subfolder}{sep}{blocktype}{sep}";

            if (!Directory.Exists(path))
            {
                return;
            }

            string[] files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return;
            }

            Dictionary<int, List<TDrawable>> dictionary = new Dictionary<int, List<TDrawable>>();
            // Screens go from 1 to 169
            Regex regex = new Regex($@"^{subfolder}(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
            foreach (string xmlFilePath in files)
            {
                string xmlFile = Path.GetFileName(xmlFilePath);
                if (!regex.IsMatch(xmlFile))
                {
                    continue;
                }

                XmlRootAttribute xmlRootAttribute = new XmlRootAttribute
                {
                    ElementName = xmlRootTag.Remove(xmlRootTag.Length - 1),
                    IsNullable = true
                };
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TDrawable), xmlRootAttribute);
                XmlDocument document = new XmlDocument();
                document.Load(xmlFilePath);

                List<TDrawable> drawables = new List<TDrawable>();
                foreach (XmlNode node in document.SelectNodes($"{xmlRootTag}/{xmlRootTag.Remove(xmlRootTag.Length - 1)}"))
                {
                    XmlNodeReader xmlNodeReader = new XmlNodeReader(node);
                    TDrawable drawable = (TDrawable)xmlSerializer.Deserialize(xmlNodeReader);
                    if (drawable.InitializeTextures(contentManager, $"{path}textures{sep}")
                        && drawable.InitializeOthers())
                    {
                        drawables.Add(drawable);
                    }
                }

                if (drawables.Count > 0)
                {
                    dictionary.Add(int.Parse(Regex.Replace(xmlFile, @"[^\d]", "")) - 1, drawables);
                }
            }

            if (dictionary.Count > 0)
            {
                drawablesDict = dictionary;
            }
        }

        protected override void Update(float p_delta)
        {
            EntityUpdate(p_delta);
        }

        public override void Draw()
        {
            if (UpdateCurrentScreen() && !EndingManager.HasFinished)
            {
                SpriteBatch spriteBatch = Game1.spriteBatch;
                EntityDraw(spriteBatch);
            }
        }

        /// <summary>
        /// Updates what screen is currently active and gets the platforms from the platform dictionary
        /// </summary>
        /// <returns>If no platforms are to be drawn false, true otherwise</returns>
        private bool UpdateCurrentScreen()
        {
            int nextScreen = Camera.CurrentScreen;
            if (currentScreen != nextScreen)
            {
                drawablesDict?.TryGetValue(nextScreen, out currentDrawables);
                currentScreen = nextScreen;
            }
            return currentDrawables != null;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating. Progress is clamped from zero to one.
        /// The amount is multiplied by two to keep parity with a previous bug.
        /// </summary>
        /// <param name="state">State of the platforms type</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        /// <returns>The new progress after updating</returns>
        protected float UpdateProgressClamped(bool state, float progress, float amount, float multiplier)
        {
            int stateInt = Convert.ToInt32(state);
            if (progress == stateInt)
            {
                return progress;
            }
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= (-1 + (stateInt * 2)) * 2 * multiplier;
            progress += amount;
            return Math.Min(Math.Max(progress, 0), 1);
        }
    }
}
