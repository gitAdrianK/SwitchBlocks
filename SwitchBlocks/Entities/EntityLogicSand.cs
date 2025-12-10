namespace SwitchBlocks.Entities
{
    using Data;
    using Settings;

    /// <summary>
    ///     Sand logic entity.
    /// </summary>
    public class EntityLogicSand : EntityLogic<DataSand>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public EntityLogicSand(SettingsSand settings) : base(DataSand.Instance, settings.Multiplier)
        {
        }

        /// <summary>
        ///     Adds delta time to the progress.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime) => this.Data.Progress += deltaTime * this.Multiplier;
    }
}
