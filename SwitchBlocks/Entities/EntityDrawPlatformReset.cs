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
                var frameIndex = this.FrameIndex;
                frameIndex.Index = 0;
            }
            this.Timer += p_delta;
            while (this.Timer > this.Frames[this.FrameIndex.Index])
            {
                this.Timer -= this.Frames[this.FrameIndex.Index];
                var frameIndex = this.FrameIndex;
                var index = frameIndex.Index;
                frameIndex.Index = index + 1;
            }
            if (this.Timer < 0f)
            {
                this.Timer = 0f;
            }
            this.Index = this.FrameIndex.Index;
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
