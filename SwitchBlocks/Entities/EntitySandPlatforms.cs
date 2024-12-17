using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Settings;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering sand platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntitySandPlatforms : EntityDrawables<PlatformSand>
    {
        private static EntitySandPlatforms instance;
        public static EntitySandPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntitySandPlatforms();
                }
                return instance;
            }
        }

        private EntitySandPlatforms()
        {
            // TODO:
            //PlatformDictionary = PlatformSand.GetPlatformsDictonary(ModStrings.SAND);
        }

        private float offset;

        public override void Reset()
        {
            instance.Destroy();
            instance = null;
        }

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
