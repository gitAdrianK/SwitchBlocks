using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Settings;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    public class EntityJumpPlatforms : EntityDrawables<PlatformInOut>
    {
        private static EntityJumpPlatforms instance;
        public static EntityJumpPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityJumpPlatforms();
                }
                return instance;
            }
        }

        private EntityJumpPlatforms()
        {
            // TODO:
            //PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.JUMP);
        }

        public override void Reset()
        {
            instance.Destroy();
            instance = null;
        }

        protected override void EntityUpdate(float p_delta)
        {
            DataJump.Progress = UpdateProgressClamped(
                DataJump.State,
                DataJump.Progress,
                p_delta,
                SettingsJump.Multiplier);
            TrySwitch();
        }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(
                    spriteBatch,
                    DataJump.State,
                    DataJump.Progress);
            });
        }

        private void TrySwitch()
        {
            if (DataJump.CanSwitchSafely && DataJump.SwitchOnceSafe)
            {
                if (IsActiveOnCurrentScreen)
                {
                    ModSounds.JumpFlip?.PlayOneShot();
                }
                DataJump.State = !DataJump.State;
                DataJump.SwitchOnceSafe = false;
            }
        }
    }
}
