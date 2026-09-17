namespace SwitchBlocks.Menus.Model
{
    using System.Collections.Generic;
    using BehaviorTree;
    using Controls;
    using EntityComponent;
    using EntityComponent.BT;
    using HarmonyLib;
    using JumpKing;
    using JumpKing.PauseMenu;
    using JumpKing.PauseMenu.BT;
    using JumpKing.PauseMenu.BT.Actions;
    using JumpKing.PauseMenu.BT.Actions.BindController;
    using JumpKing.Util;
    using LanguageJK;
    using Microsoft.Xna.Framework;
    using IDrawable = JumpKing.Util.IDrawable;

    public static class ModelMenuOptions
    {
        private static readonly AccessTools.FieldRef<object, List<IDrawable>> DrawablesRef =
            AccessTools.FieldRefAccess<object, List<IDrawable>>(
                AccessTools.Field("JumpKing.PauseMenu.MenuFactory:m_drawables"));

        private static readonly AccessTools.FieldRef<object, Entity> EntityRef =
            AccessTools.FieldRefAccess<object, Entity>(
                AccessTools.Field("JumpKing.PauseMenu.MenuFactory:m_entity"));

        private static readonly GuiFormat GuiFormatLeft =
            AccessTools.StaticFieldRefAccess<GuiFormat>("JumpKing.PauseMenu.MenuFactory:CONTROLS_GUI_FORMAT_LEFT");

        private static readonly GuiFormat GuiFormatRight =
            AccessTools.StaticFieldRefAccess<GuiFormat>("JumpKing.PauseMenu.MenuFactory:CONTROLS_GUI_FORMAT_RIGHT");

        public static BTsimultaneous CreateBindControls(object menuFactory)
        {
            var drawables = DrawablesRef(menuFactory);
            var entity = EntityRef(menuFactory);

            var menuSelector = new MenuSelector(GuiFormatLeft);

            var btSimultaneous = new BTsimultaneous();
            btSimultaneous.AddChild(menuSelector);
            drawables.Add(menuSelector);

            // left
            var menuFontSmall = Game1.instance.contentManager.font.MenuFontSmall;
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_SCAN_FOR_DEVICES, new GetSlimDevices(),
                menuFontSmall));
            menuSelector.AddChild(new SelectDevice(entity));

            var count = drawables.Count;
            var child = MakeBindController(0, entity, drawables);
            var child2 = MakeBindController(1, entity, drawables);
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_BIND_PRIMARY, child, menuFontSmall));
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_BIND_SECONDARY, child2, menuFontSmall));

            var btSequencer = new BTsequencor();
            btSequencer.AddChild(new MenuBindDefault(entity));
            btSequencer.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_DEFAULT, btSequencer, menuFontSmall));

            var btSequencer2 = new BTsequencor();
            btSequencer2.AddChild(new MenuBindSave(entity));
            btSequencer2.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            menuSelector.AddChild(new SaveNotifier(entity,
                new TextButton(language.MENUFACTORY_SAVE, btSequencer2, menuFontSmall)));

            menuSelector.Initialize();
            menuSelector.GetBounds();

            // right
            var displayFrame = new DisplayFrame(GuiFormatRight, BTresult.Running);
            displayFrame.AddChild(new MenuBindDisplay(entity, EBinding.Switch));
            displayFrame.Initialize();

            drawables.Insert(count, displayFrame);

            btSimultaneous.AddChild(new StaticNode(displayFrame, BTresult.Failure));
            return btSimultaneous;
        }

        private static IBTnode MakeBindController(int orderIndex, Entity entity, List<IDrawable> drawables)
        {
            var guiFormat = new GuiFormat
            {
                anchor_bounds = new Rectangle(0, 0, 480, 360),
                anchor = new Vector2(1f, 1f) / 2f,
                all_margin = 16,
                element_margin = 8,
                all_padding = 16,
            };

            var menuSelector = new MenuSelector(guiFormat) { AllowEscape = false };
            var child = new BindCatchSave(entity);
            var child2 = new MenuBindDefault(entity);
            var child3 = new MenuSelectorBack(menuSelector);

            var btSequencer = new BTsequencor();
            btSequencer.AddChild(child2);
            btSequencer.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            btSequencer.AddChild(child3);

            var timerAction = new TimerAction(language.MENUFACTORY_REVERTS_IN, 5, Color.Gray, btSequencer);
            menuSelector.AddChild(new TextInfo(language.MENUFACTORY_KEEPCHANGES, Color.Gray));
            menuSelector.AddChild(timerAction);
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_NO, btSequencer));
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_YES, child3));
            menuSelector.SetNodeForceRun(timerAction);
            menuSelector.Initialize(false);

            drawables.Add(menuSelector);

            var btSequencer2 = new BTsequencor();
            btSequencer2.AddChild(child);
            btSequencer2.AddChild(new WaitUntilNoMenuInput());
            btSequencer2.AddChild(MakeBindButtonMenu(EBinding.Switch, guiFormat, orderIndex, entity, drawables));
            btSequencer2.AddChild(new WaitUntilNoInput(entity));
            btSequencer2.AddChild(menuSelector);

            var btSelector = new BTselector();
            btSelector.AddChild(btSequencer2);
            btSelector.AddChild(new PlaySFX(Game1.instance.contentManager.audio.menu.MenuFail));
            return btSelector;
        }

        private static BindButtonFrame MakeBindButtonMenu(EBinding button, GuiFormat guiFormat, int orderIndex,
            Entity entity, List<IDrawable> drawables)
        {
            var btSequencer = new BTsequencor();
            btSequencer.AddChild(new WaitUntilNoInput(entity));
            btSequencer.AddChild(new MenuBindButton(entity, button, orderIndex));
            btSequencer.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));

            var bindButtonFrame = new BindButtonFrame(guiFormat, btSequencer);
            bindButtonFrame.AddChild(new TextButton(Util.ParseString(language.MENUFACTORY_PRESS_BUTTON, button),
                btSequencer));
            bindButtonFrame.Initialize();

            drawables.Add(bindButtonFrame);

            return bindButtonFrame;
        }
    }
}
