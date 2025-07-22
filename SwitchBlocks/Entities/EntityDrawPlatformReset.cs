namespace SwitchBlocks.Entities
{
    using Data;
    using JumpKing;
    using Patches;
    using Util.Deserialization;

    /// <summary>
    ///     Looping animated platform drawn based on countdown data.
    ///     Loop is reset should a lever be touched.
    /// </summary>
    public class EntityDrawPlatformReset : EntityDrawPlatformLoop
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="Platform" />.</param>
        /// <param name="screen">Screen this entity is on.</param>
        public EntityDrawPlatformReset(
            Platform platform,
            int screen)
            : base(platform, screen, DataCountdown.Instance)
            => this.Data = DataCountdown.Instance;

        /// <summary><see cref="DataCountdown" />.</summary>
        private DataCountdown Data { get; }

        /// <summary>
        ///     <inheritdoc />
        ///     Resets them should a lever be touched.
        /// </summary>
        protected override void Update(float pDelta)
        {
            if (PatchAchievementManager.GetTick() == this.Data.ActivatedTick)
            {
                this.Timer = 0;
                this.FrameIndex.Index = 0;
            }

            base.Update(pDelta);
        }

        /// <inheritdoc />
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
