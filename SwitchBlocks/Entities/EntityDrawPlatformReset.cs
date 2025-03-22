namespace SwitchBlocks.Entities
{
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util.Deserialization;

    /// <summary>
    /// Looping animated platform drawn based on countdown data.
    /// Loop is reset should a lever be touched.
    /// </summary>
    public class EntityDrawPlatformReset : EntityDrawPlatformLoop
    {
        /// <summary><see cref="DataCountdown"/>.</summary>
        private new DataCountdown Data { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="Platform"/>.</param>
        /// <param name="screen">Screen this entity is on.</param>
        public EntityDrawPlatformReset(
            Platform platform,
            int screen)
            : base(platform, screen, DataCountdown.Instance)
            => this.Data = DataCountdown.Instance;

        /// <summary>
        /// <inheritdoc/>.
        /// Resets them should a lever be touched.
        /// </summary>
        protected override void Update(float p_delta)
        {
            if (AchievementManager.GetTick() == this.Data.ActivatedTick)
            {
                this.Timer = 0;
                this.FrameIndex.Index = 0;
            }
            base.Update(p_delta);
        }

        /// <inheritdoc/>
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
