namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities.Drawables;

    /// <summary>
    /// Entity responsible for rendering countdown levers in the level.
    /// </summary>
    public class EntityCountdownLevers : EntityDrawables<Lever>
    {
        public EntityCountdownLevers() : base(ModConsts.XML_LEVERS, ModConsts.COUNTDOWN) { }

        protected override void EntityUpdate(float p_delta) { }

        public override void EntityDraw(SpriteBatch spriteBatch) => _ = Parallel.ForEach(this.CurrentDrawables, drawable
            => drawable.Draw(
                spriteBatch,
                DataCountdown.State,
                DataCountdown.Progress));
    }
}
