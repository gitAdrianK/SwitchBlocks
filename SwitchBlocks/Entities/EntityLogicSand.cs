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
        public EntityLogicSand(SettingsSand settings) : base(DataSand.Instance)
            => this.UpdateSettings(settings);

        /// <summary>
        ///     Updates the settings from the given settings.
        /// </summary>
        /// <param name="settings"><see cref="SettingsSand" />.</param>
        public void UpdateSettings(SettingsSand settings) => this.Multiplier = settings.Multiplier;

        /// <summary>
        ///     Adds delta time to the progress.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime) => this.Data.Progress += deltaTime * this.Multiplier;
    }
}
