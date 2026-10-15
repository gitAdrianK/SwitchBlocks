namespace SwitchBlocks.Menus
{
    using BehaviorTree;
    using Controls;
    using EntityComponent;
    using EntityComponent.BT;

    public class MenuBindDefault : EntityBTNode
    {
        public MenuBindDefault(Entity entity) : base(entity) { }

        protected override BTresult MyRun(TickData data)
        {
            ModEntry.Preferences.KeyBindings = new Preferences().KeyBindings;
            return BTresult.Success;
        }
    }
}
