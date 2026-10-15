namespace SwitchBlocks.Menus
{
    using BehaviorTree;
    using Controls;
    using EntityComponent;
    using EntityComponent.BT;
    using JumpKing;
    using JumpKing.Controller;
    using JumpKing.PauseMenu;
    using JumpKing.PauseMenu.BT;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class MenuBindDisplay : EntityBTNode, IMenuItem, UnSelectable
    {
        private readonly EBinding button;

        public MenuBindDisplay(Entity entity, EBinding button) : base(entity) => this.button = button;
        private SpriteFont Font => Game1.instance.contentManager.font.MenuFontSmall;

        public void Draw(int x, int y, bool selected)
        {
            var mainPad = ControllerManager.instance.GetMain();
            var display = this.button + " : ";

            var pad = mainPad.GetPad();

            MenuItemHelper.Draw(x, y, display, Color.Gray, this.Font);

            var xSize = this.GetSize().X;

            foreach (var bind in ModEntry.Preferences.KeyBindings[this.button])
            {
                x += (int)(xSize / 3f);
                display = pad.ButtonToString(bind);

                display = this.FormatString(display);
                MenuItemHelper.Draw(x, y, display, Color.Gray, this.Font);
            }

            if (ModEntry.Preferences.KeyBindings[this.button].Length != 0)
            {
                return;
            }

            x += (int)(xSize / 3f);
            display = "-";
            MenuItemHelper.Draw(x + (int)(xSize / 3f * 1f), y, display, Color.Gray, this.Font);
        }

        public Point GetSize() => MenuItemHelper.GetSize("xbox 360 controller 1____         ", this.Font);

        private string FormatString(string @string)
        {
            var num = this.GetSize().X / 3;
            if (MenuItemHelper.GetSize(@string, this.Font).X <= num)
            {
                return @string;
            }

            while (MenuItemHelper.GetSize(@string, this.Font).X > num)
            {
                @string = @string.Substring(0, @string.Length - 1);
            }

            return @string.Substring(0, @string.Length - 1) + "*";
        }

        protected override BTresult MyRun(TickData data) => BTresult.Failure;
    }
}
