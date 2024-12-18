using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering sand levers in the level.
    /// </summary>
    public class EntitySandLevers : EntityDrawables<Lever>
    {
        public EntitySandLevers() : base(ModStrings.LEVERS, ModStrings.SAND) { }

        protected override void EntityUpdate(float p_delta) { }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                // Levers don't care about progress, and sand doesn't track it.
                drawable.Draw(spriteBatch, DataSand.State, 0);
            });
        }
    }
}
