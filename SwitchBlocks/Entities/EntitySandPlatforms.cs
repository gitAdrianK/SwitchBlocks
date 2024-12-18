using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Settings;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering sand platforms in the level.
    /// </summary>
    public class EntitySandPlatforms : EntityDrawables<PlatformSand>
    {
        private float offset;

        public EntitySandPlatforms() : base(ModStrings.PLATFORMS, ModStrings.SAND) { }

        protected override void EntityUpdate(float p_delta)
        {
            offset += p_delta * SettingsSand.Multiplier;
        }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(
                    spriteBatch,
                    DataSand.State,
                    offset);
            });
        }
    }
}
