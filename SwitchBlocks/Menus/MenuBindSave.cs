namespace SwitchBlocks.Menus
{
    using BehaviorTree;
    using EntityComponent;
    using EntityComponent.BT;

    public class MenuBindSave : EntityBTNode
    {
        public MenuBindSave(Entity entity) : base(entity) { }

        protected override BTresult MyRun(TickData data)
        {
            ModEntry.Preferences.ForceUpdate();
            return BTresult.Success;
        }
    }
}
