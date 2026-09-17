namespace SwitchBlocks
{
    using System.IO;
    using Behaviours;
    using Entities;
    using JumpKing;

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
        ///     <c>true</c> if the game is in debug mode and started with the Worldsmith folder structure present, <c>false</c>
        ///     otherwise.
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                if (LevelDebugState.instance == null)
                {
                    return false;
                }

                var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
                if (directoryBin.Name != "bin" || directoryBin.Parent == null)
                {
                    return false;
                }

                var directorySaves =
                    Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder, ModConstants.Saves);
                return Directory.Exists(directorySaves);
            }
        }

        /// <summary>Logic entity of the auto block type.</summary>
        public EntityLogicAuto EntityLogicAuto { get; set; }

        /// <summary>Behaviour attached to reset.</summary>
        public BehaviourAutoReset BehaviourAutoReset { get; set; }

        /// <summary>Logic entity of the basic block type.</summary>
        public EntityLogicBasic EntityLogicBasic { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourBasicLever BehaviourBasicLever { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourBasicSingleUse BehaviourBasicSingleUse { get; set; }

        /// <summary>Behaviour attached to reset.</summary>
        public BehaviourBasicReset BehaviourBasicReset { get; set; }

        /// <summary>Logic entity of the countdown block type.</summary>
        public EntityLogicCountdown EntityLogicCountdown { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourCountdownLever BehaviourCountdownLever { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourCountdownSingleUse BehaviourCountdownSingleUse { get; set; }

        /// <summary>Behaviour attached to lever.</summary>
        public BehaviourCountdownCustomDuration BehaviourCountdownCustomDuration { get; set; }

        /// <summary>Behaviour attached to reset.</summary>
        public BehaviourCountdownReset BehaviourCountdownReset { get; set; }

        /// <summary>Logic entity of the group block type.</summary>
        public EntityLogicGroup EntityLogicGroup { get; set; }

        /// <summary>Behaviour attached to reset.</summary>
        public BehaviourGroupReset BehaviourGroupReset { get; set; }

        /// <summary>Behaviour attached to deactivate.</summary>
        public BehaviourGroupDeactivate BehaviourGroupDeactivate { get; set; }

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

        /// <summary>Default active block groups.</summary>
        public int[] DefaultActiveSequence { get; set; }

        /// <summary>Logic entity of the threshold block type.</summary>
        public EntityLogicThreshold EntityLogicThreshold { get; set; }

        /// <summary>Behaviour attached to reset.</summary>
        public BehaviourThresholdReset BehaviourThresholdReset { get; set; }

        /// <summary>Sets the singleton instance to null.</summary>
        public static void Reset() => instance = null;
    }
}
