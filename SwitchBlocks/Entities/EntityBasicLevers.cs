using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering basic levers in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityBasicLevers : EntityDrawables<Lever>
    {
        private static EntityBasicLevers instance;
        public static EntityBasicLevers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityBasicLevers();
                }
                return instance;
            }
        }

        private EntityBasicLevers()
        {
            // TODO:
            //LeverDictionary = Lever.GetLeversDictonary(ModStrings.BASIC);
            // TODO: The Platforms and levers might be able to go into one entity
        }

        public override void Reset()
        {
            instance.Destroy();
            instance = null;
        }

        protected override void EntityUpdate(float p_delta)
        {
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
