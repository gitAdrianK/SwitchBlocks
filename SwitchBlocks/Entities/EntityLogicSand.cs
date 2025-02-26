namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;

    public class EntityLogicSand : EntityLogic<DataSand>
    {
        public EntityLogicSand() : base(DataSand.Instance, SettingsSand.Multiplier)
        {
        }

        protected override void Update(float deltaTime) => this.Data.Progress += deltaTime * this.Multiplier;
    }
}
