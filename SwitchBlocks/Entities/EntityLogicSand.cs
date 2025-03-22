namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Sand logic entity.
    /// </summary>
    public class EntityLogicSand : EntityLogic<DataSand>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public EntityLogicSand() : base(DataSand.Instance, SettingsSand.Multiplier)
        {
        }

        /// <summary>
        /// Adds delta time to the progress.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime) => this.Data.Progress += deltaTime * this.Multiplier;
    }
}
