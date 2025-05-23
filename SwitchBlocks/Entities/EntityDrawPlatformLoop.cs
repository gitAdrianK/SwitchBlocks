namespace SwitchBlocks.Entities
{
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patches;
    using SwitchBlocks.Util;
    using SwitchBlocks.Util.Deserialization;

    /// <summary>
    /// Looping animated platform drawn based on data.
    /// </summary>
    public class EntityDrawPlatformLoop : EntityDrawPlatform
    {
        /// <summary>
        /// Rectangles for all sectors of the <see cref="Texture2D"/>
        /// </summary>
        protected Rectangle[] Rects { get; }
        /// <summary>Duration of every frame.</summary>
        protected float[] Frames { get; }
        /// <summary>Timer counting deltaTime.</summary>
        protected float Timer { get; set; }
        /// <summary>Frames per second, ignored if Frames is not null.</summary>
        protected float TimeStep { get; }
        /// <summary><see cref="WrappedIndex"/> providing the index of the Timer limited to its length.</summary>
        protected WrappedIndex FrameIndex { get; set; }
        /// <summary>Index limited to Rects length.</summary>
        protected int Index
        {
            get => this.InternalIndex;
            set => this.InternalIndex = value % this.Rects.Length;
        }
        /// <summary>InternalIndex.</summary>
        protected int InternalIndex { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="Platform"/>.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider"/>.</param>
        public EntityDrawPlatformLoop(
            Platform platform,
            int screen,
            IDataProvider data) : base(platform, screen, data)
        {
            var sprites = platform.Sprites;
            var cells = sprites.Cells;
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
            this.TimeStep = 1.0f / sprites.FPS;
            if (sprites.Frames == null)
            {
                this.Frames = new float[this.Rects.Length];
                for (var i = 0; i < this.Frames.Length; i++)
                {
                    this.Frames[i] = this.TimeStep;
                }
            }
            else
            {
                this.Frames = sprites.Frames;
            }
            this.FrameIndex = new WrappedIndex(this.Frames.Length);
            if (sprites.RandomOffset)
            {
                this.Timer = (float)Game1.random.NextDouble() * 100.0f;
            }
        }

        /// <summary>
        /// Updates Timer and Index.
        /// </summary>
        /// <param name="p_delta">Amount timer is increased by.</param>
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

        /// <summary>
        /// <inheritdoc/>
        /// Draws only the part of the texture given by the index.
        /// </summary>
        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || PatchEndingManager.HasFinished)
            {
                return;
            }
            this.DrawWithRectangle(this.Rects[this.Index]);
        }
    }
}
