namespace SwitchBlocks.Util.Deserialization
{
    /// <summary>
    /// Used as a XML deserialization helper
    /// </summary>
    public class Platform : Lever
    {
        public bool StartState { get; set; }

        public Animation Animation { get; set; }

        public Animation AnimationOut { get; set; }

        public Sprites Sprites { get; set; }
    }
}
