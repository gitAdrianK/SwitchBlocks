namespace SwitchBlocks
{
    using System.Collections.Generic;
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

        /// <summary>All <see cref="JKSound"/>s that have been loaded with their paths as keys.</summary>
        private static Dictionary<string, JKSound> Sounds { get; set; } = new Dictionary<string, JKSound>();

        /// <summary>
        /// Initializes the blocks <see cref="JKSound"/>s from "Steam Workshop Path\Steam\1061090\MAP ID\switchBlocksMod\audio\".
        /// Supported names are: autoFlip.xnb, autoWarn.xnb, basicFlip.xnb, countdownFlip.xnb, countdownWarn.xnb, groupFlip.xnb,
        /// jumpFlip.xnb, sandFlip.xnb and sequenceFlip.xnb.
        /// A sound can be <c>null</c>, this should be checked for before trying to play it.
        /// </summary>
        public static void Setup()
        {
            var contentManager = Game1.instance.contentManager;
            var path = Path.Combine(contentManager.root, ModConsts.FOLDER, "audio");
            if (!Directory.Exists(path))
            {
                return;
            }

            string file;
            JKSound sound;

            // Auto
            if (SetupAuto.IsUsed)
            {
                file = Path.Combine(path, "autoFlip");
                if (Sounds.TryGetValue(file, out sound))
                {
                    AutoFlip = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        AutoFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = AutoFlip;
                    }
                }
                file = Path.Combine(path, "autoWarn");
                if (Sounds.TryGetValue(file, out sound))
                {
                    AutoWarn = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        AutoWarn = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = AutoWarn;
                    }
                }
            }

            // Basic
            if (SetupBasic.IsUsed)
            {
                file = Path.Combine(path, "basicFlip");
                if (Sounds.TryGetValue(file, out sound))
                {
                    BasicFlip = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        BasicFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = BasicFlip;
                    }
                }
            }

            // Countdown
            if (SetupCountdown.IsUsed)
            {
                file = Path.Combine(path, "countdownFlip");
                if (Sounds.TryGetValue(file, out sound))
                {
                    CountdownFlip = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        CountdownFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = CountdownFlip;
                    }
                }
                file = Path.Combine(path, "countdownWarn");
                if (Sounds.TryGetValue(file, out sound))
                {
                    CountdownWarn = sound;
                }
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        CountdownWarn = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = CountdownWarn;
                    }
                }
            }

            // Group
            if (SetupGroup.IsUsed)
            {
                file = Path.Combine(path, "groupFlip");
                if (Sounds.TryGetValue(file, out sound))
                {
                    GroupFlip = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        GroupFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = GroupFlip;
                    }
                }
            }

            // Jump
            if (SetupJump.IsUsed)
            {
                file = Path.Combine(path, "jumpFlip");
                if (Sounds.TryGetValue(file, out sound))
                {
                    JumpFlip = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        JumpFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = JumpFlip;
                    }
                }
            }

            // Sand
            if (SetupSand.IsUsed)
            {
                file = Path.Combine(path, "sandFlip");
                if (Sounds.TryGetValue(file, out sound))
                {
                    SandFlip = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        SandFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = SandFlip;
                    }
                }
            }

            // Sequence
            if (SetupSequence.IsUsed)
            {
                file = Path.Combine(path, "sequenceFlip");
                if (Sounds.TryGetValue(file, out sound))
                {
                    SequenceFlip = sound;
                }
                else
                {
                    if (File.Exists(file + ".xnb"))
                    {
                        SequenceFlip = new JKSound(contentManager.Load<SoundEffect>(file), SoundType.SFX);
                        Sounds[file] = SequenceFlip;
                    }
                }
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
    }
}
