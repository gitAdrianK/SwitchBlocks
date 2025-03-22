namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Basic logic entity.
    /// </summary>
    public class EntityLogicBasic : EntityLogic<DataBasic>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public EntityLogicBasic() : base(DataBasic.Instance, SettingsBasic.Multiplier)
        {
        }

        /// <summary>
        /// Updates progress.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
            => this.UpdateProgress(this.Data.State, deltaTime);
    }
}
