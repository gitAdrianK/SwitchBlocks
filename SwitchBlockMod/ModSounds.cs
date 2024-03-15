using JumpKing;
using JumpKing.XnaWrappers;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace SwitchBlocksMod.Util
{
    /// <summary>
    /// Contains sounds used in this mod for its blocks.<br />
    /// Initializes the blocks sounds from "Steam Workshop Path\1061090\MAP ID\SwitchBlocksMod\Audio\"<br />
    /// <list type="bullet">
    ///     <item>autoBlink.xnb</item>
    ///     <item>autoFlip.xnb</item>
    ///     <item>basicFlip.xnb</item>
    ///     <item>countdownBlink.xnb</item>
    ///     <item>countdownFlip.xnb</item>
    ///     <item>sandFlip.xnb</item>
    /// </list>
    /// A sound can be null, this should be checked for before trying to play it.
    /// </summary>
    public static class ModSounds
    {
        /// <summary>
        /// Sound played when the auto block blinks.
        /// </summary>
        public static readonly JKSound AUTO_BLINK;
        /// <summary>
        /// Sound played when the auto block flips state.
        /// </summary>
        public static readonly JKSound AUTO_FLIP;

        /// <summary>
        /// Sound played when the basic block flips state.
        /// </summary>
        public static readonly JKSound BASIC_FLIP;

        /// <summary>
        /// Sound played when the countdown block blinks.
        /// </summary>
        public static readonly JKSound COUNTDOWN_BLINK;
        /// <summary>
        /// Sound played when the countdown block flips state.
        /// </summary>
        public static readonly JKSound COUNTDOWN_FLIP;

        /// <summary>
        /// Sound played when the sand block flips state.
        /// </summary>
        public static readonly JKSound SAND_FLIP;

        static ModSounds()
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{Game1.instance.contentManager.root}{sep}switchBlocksMod{sep}audio{sep}";

            // Auto
            if (File.Exists($"{path}autoBlink.xnb"))
            {
                AUTO_BLINK = new JKSound(contentManager.Load<SoundEffect>($"{path}autoBlink"), SoundType.SFX);
            }
            if (File.Exists($"{path}autoFlip.xnb"))
            {
                AUTO_FLIP = new JKSound(contentManager.Load<SoundEffect>($"{path}autoFlip"), SoundType.SFX);
            }

            // Basic
            if (File.Exists($"{path}basicFlip.xnb"))
            {
                BASIC_FLIP = new JKSound(contentManager.Load<SoundEffect>($"{path}basicFlip"), SoundType.SFX);
            }

            // Countdown
            if (File.Exists($"{path}countdownBlink.xnb"))
            {
                COUNTDOWN_BLINK = new JKSound(contentManager.Load<SoundEffect>($"{path}countdownBlink"), SoundType.SFX);
            }
            if (File.Exists($"{path}countdownFlip.xnb"))
            {
                COUNTDOWN_FLIP = new JKSound(contentManager.Load<SoundEffect>($"{path}countdownFlip"), SoundType.SFX);
            }

            // Sand
            if (File.Exists($"{path}sandFlip.xnb"))
            {
                SAND_FLIP = new JKSound(contentManager.Load<SoundEffect>($"{path}sandFlip"), SoundType.SFX);
            }
        }
    }
}
