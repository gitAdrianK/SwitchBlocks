using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Entities.Drawables
{
    public abstract class Drawable : IDrawable
    {
        protected Texture2D Texture { get; set; }
        [XmlElement("Texture")]
        public string TextureAsString { get; set; }

        public Vector2 Position { get; set; }

        public abstract void Draw(SpriteBatch spriteBatch, bool state, float progress);

        public bool InitializeTextures(JKContentManager contentManager, string path)
        {
            if (!File.Exists($"{path}{TextureAsString}.xnb"))
            {
                return false;
            }
            Texture = contentManager.Load<Texture2D>($"{path}{TextureAsString}");
            return true;
        }

        public bool InitializeOthers()
        {
            return true;
        }
    }
}
