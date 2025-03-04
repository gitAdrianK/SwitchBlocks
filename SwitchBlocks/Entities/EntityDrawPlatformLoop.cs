namespace SwitchBlocks.Entities
{
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util;

    public class EntityDrawPlatformLoop : EntityDrawPlatform
    {
        protected Rectangle[] Rects { get; }
        protected float[] Frames { get; }
        protected float Timer { get; set; }
        protected float TimeStep { get; }

        protected WrappedIndex FrameIndex { get; set; }

        protected int Index
        {
            get => this.InternalIndex;
            set => this.InternalIndex = value % this.Rects.Length;
        }
        protected int InternalIndex { get; set; }

        public EntityDrawPlatformLoop(
            Texture2D texture,
            Vector2 position,
            bool startState,
            Animation animation,
            Animation animationOut,
            int screen,
            IDataProvider logic,
            Point cells,
            float timeStep,
            float[] frames,
            bool randomOffset) : base(texture, position, startState, animation, animationOut, screen, logic)
        {
            var rects = new Rectangle[cells.X * cells.Y];
            this.Width /= cells.X;
            this.Height /= cells.Y;
            for (var i = 0; i < cells.Y; i++)
            {
                for (var j = 0; j < cells.X; j++)
                {
                    rects[(i * cells.Y) + j] = new Rectangle(
                        this.Width * j,
                        this.Height * i,
                        this.Width,
                        this.Height);
                }
            }
            this.Rects = rects;
            this.TimeStep = timeStep;
            this.Frames = frames;
            if (this.Frames == null)
            {
                this.Frames = new float[this.Rects.Length];
                for (var i = 0; i < this.Frames.Length; i++)
                {
                    this.Frames[i] = this.TimeStep;
                }
            }
            else
            {
                this.Frames = frames;
            }
            this.FrameIndex = new WrappedIndex(this.Frames.Length);
            if (randomOffset)
            {
                this.Timer = (float)Game1.random.NextDouble() * 100f;
            }
        }

        protected override void Update(float p_delta)
        {
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
