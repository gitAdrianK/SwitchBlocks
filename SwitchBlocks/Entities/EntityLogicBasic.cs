namespace SwitchBlocks.Entities
{
    using Data;
    using Settings;

    /// <summary>
    ///     Basic logic entity.
    /// </summary>
    public class EntityLogicBasic : EntityLogic<DataBasic>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public EntityLogicBasic(SettingsBasic settings) : base(DataBasic.Instance)
            => this.UpdateSettings(settings);

        /// <summary>
        ///     Updates the settings from the given settings.
        /// </summary>
        /// <param name="settings"><see cref="SettingsBasic" />.</param>
        public void UpdateSettings(SettingsBasic settings) => this.Multiplier = settings.Multiplier;

        /// <summary>
        ///     Updates progress.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
            => this.UpdateProgress(this.Data.State, deltaTime);
    }
}
