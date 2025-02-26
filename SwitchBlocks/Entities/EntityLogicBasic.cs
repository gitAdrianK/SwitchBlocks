namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;

    public class EntityLogicBasic : EntityLogic<DataBasic>
    {
        public EntityLogicBasic() : base(DataBasic.Instance, SettingsBasic.Multiplier)
        {
        }

        protected override void Update(float deltaTime)
            => this.UpdateProgress(this.Data.State, deltaTime);
    }
}
