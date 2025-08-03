namespace SwitchBlocks
{
    using System.Collections.Generic;
    using System.IO;
    using JumpKing;
    using JumpKing.XnaWrappers;
    using Microsoft.Xna.Framework.Audio;

    /// <summary>
    ///     Collection of <see cref="JKSound" />s that are used by the mod and a way to load/reset them.
    /// </summary>
    public static class ModSounds
    {
        /// <summary><see cref="JKSound" /> played when the auto block flips state.</summary>
        public static JKSound AutoFlip { get; private set; }

        /// <summary><see cref="JKSound" /> played when the auto block warns of a certain amount of time passing.</summary>
        public static JKSound AutoWarn { get; private set; }

        /// <summary><see cref="JKSound" /> played when the basic block flips state.</summary>
        public static JKSound BasicFlip { get; private set; }

        /// <summary><see cref="JKSound" /> played when the countdown block flips state.</summary>
        public static JKSound CountdownFlip { get; private set; }

        /// <summary><see cref="JKSound" /> played when the countdown block warns of a certain amount of time passing.</summary>
        public static JKSound CountdownWarn { get; private set; }

        /// <summary><see cref="JKSound" /> played when the group block flips state.</summary>
        public static JKSound GroupFlip { get; private set; }

        /// <summary><see cref="JKSound" /> played when the jump block flips state.</summary>
        public static JKSound JumpFlip { get; private set; }

        /// <summary><see cref="JKSound" /> played when the sand block flips state.</summary>
        public static JKSound SandFlip { get; private set; }

        /// <summary><see cref="JKSound" /> played when the sequence block flips state.</summary>
        public static JKSound SequenceFlip { get; private set; }

        /// <summary>All maps that have been loaded with their ulong steam ID.</summary>
        private static HashSet<ulong> LoadedMaps { get; } = new HashSet<ulong>();

        /// <summary>
        ///     Initializes the blocks <see cref="JKSound" />s from "Steam Workshop Path\Steam\1061090\MAP
        ///     ID\switchBlocksMod\audio\".
        ///     Supported names are: autoFlip.xnb, autoWarn.xnb, basicFlip.xnb, countdownFlip.xnb, countdownWarn.xnb,
        ///     groupFlip.xnb,
        ///     jumpFlip.xnb, sandFlip.xnb and sequenceFlip.xnb.
        ///     A sound can be <c>null</c>, this should be checked for before trying to play it.
        /// </summary>
        public static void Setup(ulong levelId)
        {
            var contentManager = Game1.instance.contentManager;
            var path = Path.Combine(contentManager.root, ModConstants.Folder, "audio");
            var isReload = !LoadedMaps.Add(levelId);

            if (!Directory.Exists(path))
            {
                return;
            }

            AutoFlip = LoadOrReload(contentManager, Path.Combine(path, "autoFlip"), isReload);
            AutoWarn = LoadOrReload(contentManager, Path.Combine(path, "autoWarn"), isReload);
            BasicFlip = LoadOrReload(contentManager, Path.Combine(path, "basicFlip"), isReload);
            CountdownFlip = LoadOrReload(contentManager, Path.Combine(path, "countdownFlip"), isReload);
            CountdownWarn = LoadOrReload(contentManager, Path.Combine(path, "countdownWarn"), isReload);
            GroupFlip = LoadOrReload(contentManager, Path.Combine(path, "groupFlip"), isReload);
            JumpFlip = LoadOrReload(contentManager, Path.Combine(path, "jumpFlip"), isReload);
            SandFlip = LoadOrReload(contentManager, Path.Combine(path, "sandFlip"), isReload);
            SequenceFlip = LoadOrReload(contentManager, Path.Combine(path, "sequenceFlip"), isReload);
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
        ///     Loads or reloads a <see cref="SoundEffect" /> and returns a new <see cref="JKSound" /> of <see cref="SoundType" />
        ///     SFX.
        /// </summary>
        /// <param name="contentManager">JKContentManager.</param>
        /// <param name="file">Absolute path to the to be loaded file.</param>
        /// <param name="isReload">If the <see cref="SoundEffect" /> should be reloaded.</param>
        /// <returns>New <see cref="JKSound" /> or <c>null</c> should the file not exist.</returns>
        private static JKSound LoadOrReload(JKContentManager contentManager, string file, bool isReload)
        {
            if (!File.Exists(file + ".xnb"))
            {
                return null;
            }

            return new JKSound(contentManager.LoadOrReloadWrapper<SoundEffect>(file, isReload, isAbsolute: true),
                SoundType.SFX);
        }
    }
}
