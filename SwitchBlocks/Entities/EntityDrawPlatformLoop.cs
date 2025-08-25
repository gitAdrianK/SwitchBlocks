namespace SwitchBlocks.Entities
{
    using System;
    using System.Linq;
    using Data;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Patches;
    using Util;
    using Util.Deserialization;

    /// <summary>
    ///     Looping animated platform drawn based on data.
    /// </summary>
    public class EntityDrawPlatformLoop : EntityDrawPlatform
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="Platform" />.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
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
                    rects[(i * cells.X) + j] = new Rectangle(
                        this.Width * j,
                        this.Height * i,
                        this.Width,
                        this.Height);
                }
            }

            this.Rects = rects;
            this.TimeStep = (int)((1.0f / sprites.Fps / ModConstants.DeltaTime) + 0.5f);
            if (sprites.Frames == null)
            {
                this.Frames = new int[this.Rects.Length];
                for (var i = 0; i < this.Frames.Length; i++)
                {
                    this.Frames[i] = this.TimeStep;
                }
            }
            else
            {
                this.Frames = sprites.Frames
                    .Select(f => (int)((f/ ModConstants.DeltaTime) + 0.5f))
                    .ToArray();
            }

            this.FrameIndex = new WrappedIndex(this.Frames.Length);
            if (sprites.RandomOffset)
            {
                this.Timer = Game1.random.Next(100);
            }
        }

        /// <summary>
        ///     Rectangles for all sectors of the <see cref="Texture2D" />
        /// </summary>
        protected Rectangle[] Rects { get; }

        /// <summary>Duration of every frame.</summary>
        private int[] Frames { get; }

        /// <summary>Timer counting deltaTime.</summary>
        protected int Timer { get; set; }

        /// <summary>Frames per second, ignored if Frames is not null.</summary>
        private int TimeStep { get; }

        /// <summary><see cref="WrappedIndex" /> providing the index of the Timer limited to its length.</summary>
        protected WrappedIndex FrameIndex { get; }

        /// <summary>Index limited to Rects length.</summary>
        protected int Index
        {
            get => this.InternalIndex;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.InternalIndex = value % this.Rects.Length;
            }
        }

        /// <summary>InternalIndex.</summary>
        private int InternalIndex { get; set; }

        protected int PrevTick { get; set; }

        /// <summary>
        ///     Updates Timer and Index.
        /// </summary>
        /// <param name="delta">Amount timer is increased by.</param>
        protected override void Update(float delta)
        {
            var tick = PatchAchievementManager.GetTick()  -  this.Data.Tick;
            // I would have thought it's ==, but apparently not?
            // It might be because start state "on" means it starts visible,
            // and the default state is false, so visible("on") == false
            if (this.StartState != this.Data.State && !this.Data.SwitchOnceSafe)
            {
                this.Timer += tick - this.PrevTick;
                this.PrevTick = tick;
            }

            while (this.Timer > this.Frames[this.FrameIndex.Index])
            {
                this.Timer -= this.Frames[this.FrameIndex.Index];
                var index = this.FrameIndex.Index;
                this.FrameIndex.Index = index + 1;
            }

            if (this.Timer < 0)
            {
                this.Timer = 0;
            }

            this.Index = this.FrameIndex.Index;
        }

        /// <summary>
        ///     <inheritdoc />
        ///     Draws only the part of the texture given by the index.
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
