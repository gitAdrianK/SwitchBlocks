namespace SwitchBlocks
{
    using Behaviours;
    using Entities;
    using JetBrains.Annotations;
    using JumpKing;
    using JumpKing.Mods;
    using JumpKing.PauseMenu;
    using JumpKing.PauseMenu.BT;
    using Menus;

    /// <summary>
    ///     Adds some menu items only available when in a debug run.
    ///     Keeps some references around needed for debug menu items.
    /// </summary>
    public class ModDebug
    {
        /// <summary>Singleton instance.</summary>
        private static ModDebug instance;

        /// <summary>
        ///     Private ctor.
        /// </summary>
        private ModDebug() { }

        /// <summary>Gets the singleton instance.</summary>
        public static ModDebug Instance => instance ?? (instance = new ModDebug());

        /// <summary>
        ///     <c>true</c> if the game is in debug mode, <c>false</c> otherwise.
        /// </summary>
        public static bool IsDebug => LevelDebugState.instance != null;

        /// <summary>Logic entity of the auto block type.</summary>
        public EntityLogicAuto EntityLogicAuto { get; set; }

        /// <summary>Logic entity of the basic block type.</summary>
        public EntityLogicBasic EntityLogicBasic { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourBasicLever BehaviourBasicLever { get; set; }

        /// <summary>Logic entity of the countdown block type.</summary>
        public EntityLogicCountdown EntityLogicCountdown { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourCountdownLever BehaviourCountdownLever { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourCountdownSingleUse BehaviourCountdownSingleUse { get; set; }

        /// <summary>Logic entity of the group block type.</summary>
        public EntityLogicGroup EntityLogicGroup { get; set; }

        /// <summary>Behaviour attached to reset.</summary>
        public BehaviourGroupReset BehaviourGroupReset { get; set; }

        /// <summary>Logic entity of the jump block type.</summary>
        public EntityLogicJump EntityLogicJump { get; set; }

        /// <summary>Logic entity of the sand block type.</summary>
        public EntityLogicSand EntityLogicSand { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourSandLever BehaviourSandLever { get; set; }

        /// <summary>Logic entity of the sequence block type.</summary>
        public EntityLogicSequence EntityLogicSequence { get; set; }

        /// <summary>Behaviour attached to reset.</summary>
        public BehaviourSequenceReset BehaviourSequenceReset { get; set; }

        /// <summary>
        ///     Adds the debug menu item to create the blocks.xml
        /// </summary>
        /// <returns>Create blocks.xml <see cref="TextButton" /></returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton CreateBlocksXml(object factory, GuiFormat format) =>
            IsDebug
                ? new TextButton("Create blocks.xml", new NodeCreateBlocksXml())
                : null;

        /// <summary>
        ///     Adds the debug menu item to reload the blocks.xml
        /// </summary>
        /// <returns>Reload blocks.xml <see cref="TextButton" /></returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton ReloadBlocksXml(object factory, GuiFormat format) =>
            IsDebug
                ? new TextButton("Reload blocks.xml", new NodeReloadBlocksXml())
                : null;

        /// <summary>
        ///     Adds the debug menu item to reload drawables
        /// </summary>
        /// <returns>Reload drawables <see cref="TextButton" /></returns>
        [PauseMenuItemSetting]
        [UsedImplicitly]
        public static TextButton ReloadDrawables(object factory, GuiFormat format) =>
            IsDebug
                ? new TextButton("Reload drawables", new NodeReloadDrawables())
                : null;

        /// <summary>Sets the singleton instance to null.</summary>
        public static void Reset() => instance = null;
    }
}
