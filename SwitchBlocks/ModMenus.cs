namespace SwitchBlocks
{
    using JetBrains.Annotations;
    using JumpKing.Mods;
    using JumpKing.PauseMenu;
    using JumpKing.PauseMenu.BT;
    using Menus;
    using Menus.Model;

    /// <summary>
    ///     List all menus created by the mod.
    ///     As the assembly is scanned for the annotations, placing them in a single class ensures the order of items.
    /// </summary>
    public static class ModMenus
    {
        /// <summary>
        ///     Adds the menu item to bind the switch key.
        /// </summary>
        /// <returns>Bind key <see cref="TextButton" />.</returns>
        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton BindSettings(object factory, GuiFormat format) =>
            new TextButton("Bind Key", ModelMenuOptions.CreateBindControls(factory));

        /// <summary>
        ///     Adds the debug menu item to reload the blocks.xml.
        /// </summary>
        /// <returns>Reload blocks.xml <see cref="TextButton" />.</returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton ReloadBlocksXml(object factory, GuiFormat format) =>
            ModDebug.IsDebug
                ? new TextButton("Reload blocks.xml", new NodeReloadBlocksXml())
                : null;

        /// <summary>
        ///     Adds the debug menu item to reload drawables.
        /// </summary>
        /// <returns>Reload drawables <see cref="TextButton" />.</returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton ReloadDrawables(object factory, GuiFormat format) =>
            ModDebug.IsDebug
                ? new TextButton("Reload drawables", new NodeReloadDrawables())
                : null;

        /// <summary>
        ///     Adds the debug menu item to reload seeds.
        /// </summary>
        /// <returns>Reload seeds <see cref="TextButton" />.</returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton ReloadSeeds(object factory, GuiFormat format) =>
            ModDebug.IsDebug
                ? new TextButton("Reload seeds", new NodeReloadSeeds())
                : null;

        /// <summary>
        ///     Adds the debug menu item to create mod folders.
        /// </summary>
        /// <returns>Create mod folders <see cref="TextButton" />.</returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton CreateModFolders(object factory, GuiFormat format) =>
            ModDebug.IsDebug
                ? new TextButton("Create mod folders", new NodeCreateModFolders())
                : null;

        /// <summary>
        ///     Adds the debug menu item to create the blocks.xml.
        /// </summary>
        /// <returns>Create blocks.xml <see cref="TextButton" />.</returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton CreateBlocksXml(object factory, GuiFormat format) =>
            ModDebug.IsDebug
                ? new TextButton("Create blocks.xml", new NodeCreateBlocksXml())
                : null;

        /// <summary>
        ///     Adds the debug menu item to create templates.
        /// </summary>
        /// <returns>Create templates <see cref="TextButton" />.</returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton CreateTemplates(object factory, GuiFormat format) =>
            ModDebug.IsDebug
                ? new TextButton("Create drawables templates", new NodeCreateTemplates())
                : null;
    }
}
