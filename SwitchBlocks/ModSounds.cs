namespace SwitchBlocks
{
    using System.IO;
    using JumpKing;
    using JumpKing.XnaWrappers;
    using Microsoft.Xna.Framework.Audio;
    using SwitchBlocks.Setups;

    /// <summary>
    /// Contains sounds used in this mod for its blocks.<br />
    /// Initializes the blocks sounds from "Steam Workshop Path\1061090\MAP ID\switchBlocksMod\audio\"<br />
    /// <list type="bullet">
    ///     <item>autoFlip.xnb</item>
    ///     <item>autoWarn.xnb</item>
    ///     <item>basicFlip.xnb</item>
    ///     <item>countdownFlip.xnb</item>
    ///     <item>countdownWarn.xnb</item>
    ///     <item>groupFlip.xnb</item>    
    ///     <item>jumpFlip.xnb</item>
    ///     <item>sandFlip.xnb</item>
    ///     <item>sequenceFlip.xnb</item>
    ///     <item>windFlip.xnb</item>
    /// </list>
    /// A sound can be null, this should be checked for before trying to play it.
    /// </summary>
    public static class ModSounds
    {
        /// <summary>
        /// Sound played when the auto block flips state.
        /// </summary>
        public static JKSound AutoFlip { get; private set; }
        /// <summary>
        /// Sound played when the auto block warns of a certain amount of time passing.
        /// </summary>
        public static JKSound AutoWarn { get; private set; }

        /// <summary>
        /// Sound played when the basic block flips state.
        /// </summary>
        public static JKSound BasicFlip { get; private set; }

        /// <summary>
        /// Sound played when the countdown block flips state.
        /// </summary>
        public static JKSound CountdownFlip { get; private set; }
        /// <summary>
        /// Sound played when the countdown block warns of a certain amount of time passing.
        /// </summary>
        public static JKSound CountdownWarn { get; private set; }

        /// <summary>
        /// Sound played when the group block flips state.
        /// </summary>
        public static JKSound GroupFlip { get; private set; }

        /// <summary>
        /// Sound played when the jump block flips state.
        /// </summary>
        public static JKSound JumpFlip { get; private set; }

        /// <summary>
        /// Sound played when the sand block flips state.
        /// </summary>
        public static JKSound SandFlip { get; private set; }

        /// <summary>
        /// Sound played when the sequence block flips state.
        /// </summary>
        public static JKSound SequenceFlip { get; private set; }

        /// <summary>
        /// Tries to load sounds used in the mod.
        /// </summary>
        public static void Load()
        {
            var contentManager = Game1.instance.contentManager;
            var path = Path.Combine(contentManager.root, ModStrings.FOLDER, "audio");
            string file;

            // Auto
            if (SetupAuto.IsUsed)
            {
                file = Path.Combine(path, "autoFlip");
                if (File.Exists(file + ".xnb"))
                {
                    AutoFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
                file = Path.Combine(path, "autoWarn");
                if (File.Exists(file + ".xnb"))
                {
                    AutoWarn = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
            }

            // Basic
            if (SetupBasic.IsUsed)
            {
                file = Path.Combine(path, "basicFlip");
                if (File.Exists(file + ".xnb"))
                {
                    BasicFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
            }

            // Countdown
            if (SetupCountdown.IsUsed)
            {
                file = Path.Combine(path, "countdownFlip");
                if (File.Exists(file + ".xnb"))
                {
                    CountdownFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
                file = Path.Combine(path, "countdownWarn");
                if (File.Exists(file + ".xnb"))
                {
                    CountdownWarn = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
            }

            // Group
            if (SetupGroup.IsUsed)
            {
                file = Path.Combine(path, "groupFlip");
                if (File.Exists(file + ".xnb"))
                {
                    GroupFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
            }

            // Jump
            if (SetupJump.IsUsed)
            {
                file = Path.Combine(path, "jumpFlip");
                if (File.Exists(file + ".xnb"))
                {
                    JumpFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
            }

            // Sand
            if (SetupSand.IsUsed)
            {
                file = Path.Combine(path, "sandFlip");
                if (File.Exists(file + ".xnb"))
                {
                    SandFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
            }

            // Sequence
            if (SetupSequence.IsUsed)
            {
                file = Path.Combine(path, "sequenceFlip");
                if (File.Exists(file + ".xnb"))
                {
                    SequenceFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                }
            }
        }

        public static void Cleanup()
        {
            AutoFlip = null;
            AutoWarn = null;
            BasicFlip = null;
            CountdownFlip = null;
            CountdownWarn = null;
            GroupFlip = null;
            JumpFlip = null;
            SandFlip = null;
            SequenceFlip = null;
        }
    }
}
