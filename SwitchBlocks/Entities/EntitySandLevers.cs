namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities.Drawables;

    /// <summary>
    /// Entity responsible for rendering sand levers in the level.
    /// </summary>
    public class EntitySandLevers : EntityDrawables<Lever>
    {
        public EntitySandLevers() : base(ModStrings.XML_LEVERS, ModStrings.SAND) { }

        protected override void EntityUpdate(float p_delta) { }

        public override void EntityDraw(SpriteBatch spriteBatch) => Parallel.ForEach(this.CurrentDrawables, drawable
            => drawable.Draw(spriteBatch, DataSand.State, 0));
    }
}
