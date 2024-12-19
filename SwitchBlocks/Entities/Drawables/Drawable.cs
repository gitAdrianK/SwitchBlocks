namespace SwitchBlocks.Entities.Drawables
{
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Drawable : IDrawable
    {
        protected Texture2D Texture { get; set; }
        [XmlElement("Texture")]
        public string TextureAsString { get; set; }

        public Vector2 Position { get; set; }

        public abstract void Draw(SpriteBatch spriteBatch, bool state, float progress);

        public virtual bool InitializeTextures(JKContentManager contentManager, string path)
        {
            if (!File.Exists($"{path}{this.TextureAsString}.xnb"))
            {
                return false;
            }
            this.Texture = contentManager.Load<Texture2D>($"{path}{this.TextureAsString}");
            return true;
        }

        public virtual bool InitializeOthers() => true;
    }
}
