using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering basic levers in the level.
    /// </summary>
    public class EntityBasicLevers : EntityDrawables<Lever>
    {
        public EntityBasicLevers() : base(ModStrings.XML_LEVERS, ModStrings.BASIC) { }

        protected override void EntityUpdate(float p_delta) { }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(spriteBatch, DataBasic.State, DataBasic.Progress);
            });
        }
    }
}
