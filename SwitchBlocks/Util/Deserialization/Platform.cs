namespace SwitchBlocks.Util.Deserialization
{
    using Entities;

    /// <summary>
    ///     Deserialization helper to create <see cref="EntityDrawPlatform" />,
    ///     <see cref="EntityDrawPlatformLoop" /> or <see cref="EntityDrawPlatformReset" />.
    /// </summary>
    public class Platform : Lever
    {
        /// <summary>Start state.</summary>
        public bool StartState { get; set; }

        /// <summary><see cref="Util.Animation" />.</summary>
        public Animation Animation { get; set; }

        /// <summary>Out <see cref="Util.Animation" />.</summary>
        public Animation AnimationOut { get; set; }

        /// <summary><see cref="Deserialization.Sprites" />.</summary>
        public Sprites Sprites { get; set; }
    }
}
