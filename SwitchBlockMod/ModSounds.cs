using JumpKing;
using JumpKing.XnaWrappers;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace SwitchBlocksMod
{
    /// <summary>
    /// Contains sounds used in this mod for its blocks.<br />
    /// Initializes the blocks sounds from "Steam Workshop Path\1061090\MAP ID\switchBlocksMod\audio\"<br />
    /// <list type="bullet">
    ///     <item>autoBlink.xnb</item>
    ///     <item>autoFlip.xnb</item>
    ///     <item>basicFlip.xnb</item>
    ///     <item>countdownBlink.xnb</item>
    ///     <item>countdownFlip.xnb</item>
    ///     <item>jumpFlip.xnb</item>
    ///     <item>sandFlip.xnb</item>
    /// </list>
    /// A sound can be null, this should be checked for before trying to play it.
    /// </summary>
    public static class ModSounds
    {
        /// <summary>
        /// Sound played when the auto block blinks.
        /// </summary>
        public static JKSound autoBlink;
        /// <summary>
        /// Sound played when the auto block flips state.
        /// </summary>
        public static JKSound autoFlip;

        /// <summary>
        /// Sound played when the basic block flips state.
        /// </summary>
        public static JKSound basicFlip;

        /// <summary>
        /// Sound played when the countdown block blinks.
        /// </summary>
        public static JKSound countdownBlink;
        /// <summary>
        /// Sound played when the countdown block flips state.
        /// </summary>
        public static JKSound countdownFlip;

        /// <summary>
        /// Sound played when the jump block flips state.
        /// </summary>
        public static JKSound jumpFlip;

        /// <summary>
        /// Sound played when the sand block flips state.
        /// </summary>
        public static JKSound sandFlip;

        /// <summary>
        /// Tries to load sounds used in the mod.
        /// </summary>
        public static void Load()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{Game1.instance.contentManager.root}{sep}{ModStrings.FOLDER}{sep}audio{sep}";

            // Auto
            if (File.Exists($"{path}autoBlink.xnb"))
            {
                autoBlink = new JKSound(contentManager.Load<SoundEffect>($"{path}autoBlink"), SoundType.SFX);
            }
            if (File.Exists($"{path}autoFlip.xnb"))
            {
                autoFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}autoFlip"), SoundType.SFX);
            }

            // Basic
            if (File.Exists($"{path}basicFlip.xnb"))
            {
                basicFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}basicFlip"), SoundType.SFX);
            }

            // Countdown
            if (File.Exists($"{path}countdownBlink.xnb"))
            {
                countdownBlink = new JKSound(contentManager.Load<SoundEffect>($"{path}countdownBlink"), SoundType.SFX);
            }
            if (File.Exists($"{path}countdownFlip.xnb"))
            {
                countdownFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}countdownFlip"), SoundType.SFX);
            }

            // Jump
            if (File.Exists($"{path}jumpFlip.xnb"))
            {
                jumpFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}jumpFlip"), SoundType.SFX);
            }

            // Sand
            if (File.Exists($"{path}sandFlip.xnb"))
            {
                sandFlip = new JKSound(contentManager.Load<SoundEffect>($"{path}sandFlip"), SoundType.SFX);
            }
        }
    }
}
