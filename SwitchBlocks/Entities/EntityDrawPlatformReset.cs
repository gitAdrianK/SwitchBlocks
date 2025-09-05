namespace SwitchBlocks.Entities
{
    using Data;
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
        /// <param name="data"><see cref="IDataProvider" />.</param>
        public EntityDrawPlatformReset(
            Platform platform,
            int screen,
            IDataProvider data)
            : base(platform, screen, data)
        {
        }

        /// <summary>
        ///     <inheritdoc />
        ///     Resets them should a lever be touched.
        /// </summary>
        protected override void Update(float delta)
        {
            if (PatchAchievementManager.GetTick() == this.Data.Tick)
            {
                this.Timer = 0;
                this.FrameIndex.Index = 0;
                this.PrevTick = this.Data.Tick - 1;
            }

            base.Update(delta);
        }
    }
}
