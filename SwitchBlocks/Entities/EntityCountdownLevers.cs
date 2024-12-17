using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering countdown levers in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityCountdownLevers : EntityDrawables<Lever>
    {
        private static EntityCountdownLevers instance;
        public static EntityCountdownLevers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityCountdownLevers();
                }
                return instance;
            }
        }

        private EntityCountdownLevers()
        {
            // TODO:
            //LeverDictionary = Lever.GetLeversDictonary(ModStrings.BASIC);
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
                drawable.Draw(spriteBatch, DataCountdown.State, DataCountdown.Progress);
            });
        }
    }
}
