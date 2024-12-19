using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering countdown levers in the level.
    /// </summary>
    public class EntityCountdownLevers : EntityDrawables<Lever>
    {
        public EntityCountdownLevers() : base(ModStrings.XML_LEVERS, ModStrings.COUNTDOWN) { }

        protected override void EntityUpdate(float p_delta) { }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(
                    spriteBatch,
                    DataCountdown.State,
                    DataCountdown.Progress);
            });
        }
    }
}
