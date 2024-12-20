namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities.Drawables;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Entity responsible for rendering sand platforms in the level.
    /// </summary>
    public class EntitySandPlatforms : EntityDrawables<PlatformSand>
    {
        private float offset;

        public EntitySandPlatforms() : base(ModConsts.XML_PLATFORMS, ModConsts.SAND) { }

        protected override void EntityUpdate(float p_delta) => this.offset += p_delta * SettingsSand.Multiplier;

        public override void EntityDraw(SpriteBatch spriteBatch) => Parallel.ForEach(this.CurrentDrawables, drawable
            => drawable.Draw(
                spriteBatch,
                DataSand.State,
                this.offset));
    }
}
