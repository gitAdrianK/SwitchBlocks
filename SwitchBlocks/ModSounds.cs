namespace SwitchBlocks
{
    using System.IO;
    using JumpKing;
    using JumpKing.XnaWrappers;
    using Microsoft.Xna.Framework.Audio;
    using SwitchBlocks.Setups;

    /// <summary>
    /// Collection of <see cref="JKSound"/>s that are used by the mod and a way to load/reset them.
    /// </summary>
    public static class ModSounds
    {
        /// <summary><see cref="JKSound"/> played when the auto block flips state.</summary>
        public static JKSound AutoFlip { get; private set; }
        /// <summary><see cref="JKSound"/> played when the auto block warns of a certain amount of time passing.</summary>
        public static JKSound AutoWarn { get; private set; }

        /// <summary><see cref="JKSound"/> played when the basic block flips state.</summary>
        public static JKSound BasicFlip { get; private set; }

        /// <summary><see cref="JKSound"/> played when the countdown block flips state.</summary>
        public static JKSound CountdownFlip { get; private set; }
        /// <summary><see cref="JKSound"/> played when the countdown block warns of a certain amount of time passing.</summary>
        public static JKSound CountdownWarn { get; private set; }

        /// <summary><see cref="JKSound"/> played when the group block flips state.</summary>
        public static JKSound GroupFlip { get; private set; }

        /// <summary><see cref="JKSound"/> played when the jump block flips state.</summary>
        public static JKSound JumpFlip { get; private set; }

        /// <summary><see cref="JKSound"/> played when the sand block flips state.</summary>
        public static JKSound SandFlip { get; private set; }

        /// <summary><see cref="JKSound"/> played when the sequence block flips state.</summary>
        public static JKSound SequenceFlip { get; private set; }

        /// <summary>
        /// Initializes the blocks <see cref="JKSound"/>s from "Steam Workshop Path\Steam\1061090\MAP ID\switchBlocksMod\audio\".
        /// Supported names are: autoFlip.xnb, autoWarn.xnb, basicFlip.xnb, countdownFlip.xnb, countdownWarn.xnb, groupFlip.xnb,
        /// jumpFlip.xnb, sandFlip.xnb and sequenceFlip.xnb.
        /// A sound can be <c>null</c>, this should be checked for before trying to play it.
        /// </summary>
        public static void Setup()
        {
            var contentManager = Game1.instance.contentManager;
            // Just the path inside the mod structure. Add root for full path.
            // Only add with "\\" or reload doesn't work.
            var innerPath = Path.Combine(ModConsts.FOLDER, "audio");
            if (!Directory.Exists(contentManager.root + "\\" + innerPath))
            {
                return;
            }

            // Auto
            if (SetupAuto.IsUsed)
            {
                AutoFlip = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "autoFlip"));
                AutoWarn = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "autoWarn"));
            }

            // Basic
            if (SetupBasic.IsUsed)
            {
                BasicFlip = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "basicFlip"));
            }

            // Countdown
            if (SetupCountdown.IsUsed)
            {
                CountdownFlip = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "countdownFlip"));
                CountdownWarn = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "countdownWarn"));
            }

            // Group
            if (SetupGroup.IsUsed)
            {
                GroupFlip = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "groupFlip"));
            }

            // Jump
            if (SetupJump.IsUsed)
            {
                JumpFlip = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "jumpFlip"));
            }

            // Sand
            if (SetupSand.IsUsed)
            {
                SandFlip = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "sandFlip"));
            }

            // Sequence
            if (SetupSequence.IsUsed)
            {
                SequenceFlip = LoadAndReloadSound(contentManager, Path.Combine(innerPath, "sequenceFlip"));
            }
        }

        /// <summary>Sets all sounds to null.</summary>
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

        /// <summary>
        /// Loads and reloads a <see cref="JKSound"/> should the file exist.
        /// </summary>
        /// <param name="contentManager"><see cref="JKContentManager"/>></param>
        /// <param name="file">Path to and name of the file, starting from the internal mod folder.</param>
        /// <returns>The <see cref="JKSound"/> if the file was found, <c>null</c> otherwise.</returns>
        private static JKSound LoadAndReloadSound(JKContentManager contentManager, string file)
        {
            if (File.Exists(contentManager.root + "\\" + file + ".xnb"))
            {
                var soundEffect = contentManager.Load<SoundEffect>(contentManager.root + "\\" + file);
                contentManager.ReloadAsset<SoundEffect>(file);
                return new JKSound(soundEffect, SoundType.SFX);
            }
            return null;
        }
    }
}
