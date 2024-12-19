using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Settings;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering basic platforms in the level.
    /// </summary>
    public class EntityBasicPlatforms : EntityDrawables<PlatformInOut>
    {
        public EntityBasicPlatforms() : base(ModStrings.XML_PLATFORMS, ModStrings.BASIC) { }

        protected override void EntityUpdate(float p_delta)
        {
            DataBasic.Progress = UpdateProgressClamped(
                DataBasic.State,
                DataBasic.Progress,
                p_delta,
                SettingsBasic.Multiplier);
        }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(
                    spriteBatch,
                    DataBasic.State,
                    DataBasic.Progress);
            });
        }
    }
}
