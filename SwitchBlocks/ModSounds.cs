using JumpKing;
using JumpKing.XnaWrappers;
using Microsoft.Xna.Framework.Audio;
using SwitchBlocks.Settings;
using System.IO;

namespace SwitchBlocks
{
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
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}audio{sep}";

            // Auto
            if (SettingsAuto.IsUsed)
            {
                if (File.Exists($"{path}autoFlip.xnb"))
                {
                    AutoFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}autoFlip"), SoundType.SFX);
                }
                if (File.Exists($"{path}autoWarn.xnb"))
                {
                    AutoWarn = new JKSound(contentManager.Load<SoundEffect>($"{path}autoWarn"), SoundType.SFX);
                }
            }

            // Basic
            if (SettingsBasic.IsUsed && File.Exists($"{path}basicFlip.xnb"))
            {
                BasicFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}basicFlip"), SoundType.SFX);
            }

            // Countdown
            if (SettingsCountdown.IsUsed)
            {
                if (File.Exists($"{path}countdownFlip.xnb"))
                {
                    CountdownFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}countdownFlip"), SoundType.SFX);
                }
                if (File.Exists($"{path}countdownWarn.xnb"))
                {
                    CountdownWarn = new JKSound(contentManager.Load<SoundEffect>($"{path}countdownWarn"), SoundType.SFX);
                }
            }

            // Group
            if (SettingsGroup.IsUsed && File.Exists($"{path}groupFlip.xnb"))
            {
                GroupFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}groupFlip"), SoundType.SFX);
            }

            // Jump
            if (SettingsJump.IsUsed && File.Exists($"{path}jumpFlip.xnb"))
            {
                JumpFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}jumpFlip"), SoundType.SFX);
            }

            // Sand
            if (SettingsSand.IsUsed && File.Exists($"{path}sandFlip.xnb"))
            {
                SandFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}sandFlip"), SoundType.SFX);
            }

            // Sequence
            if (SettingsSequence.IsUsed && File.Exists($"{path}sequenceFlip.xnb"))
            {
                SequenceFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}sequenceFlip"), SoundType.SFX);
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
