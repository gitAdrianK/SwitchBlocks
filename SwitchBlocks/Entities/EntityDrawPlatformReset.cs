namespace SwitchBlocks.Entities
{
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util;

    public class EntityDrawPlatformReset : EntityDrawPlatformLoop
    {
        private DataCountdown Data { get; }

        public EntityDrawPlatformReset(
            Texture2D texture,
            Vector2 position,
            bool startState,
            Animation animation,
            Animation animationOut,
            int screen,
            Point cells,
            float timeStep,
            float[] frames,
            bool randomOffset)
            : base(texture, position, startState, animation, animationOut, screen, DataCountdown.Instance, cells, timeStep, frames, randomOffset)
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
