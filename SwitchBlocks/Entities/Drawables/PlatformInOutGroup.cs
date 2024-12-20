namespace SwitchBlocks.Entities.Drawables
{
    public class PlatformInOutGroup : PlatformInOut
    {
        public struct JKPoint
        {
            public byte Screen { get; set; }
            public byte X { get; set; }
            public byte Y { get; set; }
        }

        public int GroupId { get; set; }

        public JKPoint Link { get; set; }

        /// <summary>
        /// Formats the link to be of the 0...0SSSXXYY format used in linking groups.
        /// </summary>
        public int FormatLink => (this.Link.Screen * 10000) + (this.Link.X * 100) + this.Link.Y;

        /// <summary>
        /// Formats the position to be of the 0...0SSSXXYY format used in linking groups.
        /// </summary>
        /// <param name="screen">The screen of the platform as this is not saved on the platform itself</param>
        public int FormatPosition(int screen) => (screen * 10000) + ((int)this.Position.X / 8 * 100) + ((int)this.Position.Y / 8);
    }
}
