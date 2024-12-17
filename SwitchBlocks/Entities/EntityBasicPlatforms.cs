using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Settings;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering basic platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityBasicPlatforms : EntityDrawables<PlatformInOut>
    {
        private static EntityBasicPlatforms instance;
        public static EntityBasicPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityBasicPlatforms();
                }
                return instance;
            }
        }

        private EntityBasicPlatforms()
        {
            // TODO:
            //PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.BASIC);
        }

        public override void Reset()
        {
            instance.Destroy();
            instance = null;
        }

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
                drawable.Draw(spriteBatch, DataBasic.State, DataBasic.Progress);
            });
        }
    }
}
