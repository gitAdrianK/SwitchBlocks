namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities.Drawables;
    using SwitchBlocks.Settings;

    public class EntityJumpPlatforms : EntityDrawables<PlatformInOut>
    {
        public EntityJumpPlatforms() : base(ModConsts.XML_PLATFORMS, ModConsts.JUMP) { }

        protected override void EntityUpdate(float p_delta)
        {
            DataJump.Progress = this.UpdateProgressClamped(
                DataJump.State,
                DataJump.Progress,
                p_delta,
                SettingsJump.Multiplier);
            this.TrySwitch();
        }

        public override void EntityDraw(SpriteBatch spriteBatch) => Parallel.ForEach(this.CurrentDrawables, drawable
            => drawable.Draw(
                spriteBatch,
                DataJump.State,
                DataJump.Progress));

        private void TrySwitch()
        {
            if (DataJump.CanSwitchSafely && DataJump.SwitchOnceSafe)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.JumpFlip?.PlayOneShot();
                }
                DataJump.State = !DataJump.State;
                DataJump.SwitchOnceSafe = false;
            }
        }
    }
}
