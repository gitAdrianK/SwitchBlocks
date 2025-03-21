namespace SwitchBlocks.Entities
{
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util.Deserialization;

    public class EntityDrawPlatformReset : EntityDrawPlatformLoop
    {
        private new DataCountdown Data { get; }

        public EntityDrawPlatformReset(
            Platform platform,
            int screen)
            : base(platform, screen, DataCountdown.Instance)
            => this.Data = DataCountdown.Instance;

        protected override void Update(float p_delta)
        {
            if (AchievementManager.GetTicks() == this.Data.ActivatedTick)
            {
                this.Timer = 0;
                this.FrameIndex.Index = 0;
            }
            base.Update(p_delta);
        }

        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || EndingManager.HasFinished)
            {
                return;
            }
            this.DrawWithRectangle(this.Rects[this.Index]);
        }
    }
}
