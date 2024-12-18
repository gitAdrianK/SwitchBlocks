namespace SwitchBlocks.Entities.Drawables
{
    public class PlatformInOutGroup : PlatformInOut
    {
        public int GroupId { get; set; }

        public new bool InitializeOthers()
        {
            if (!base.InitializeOthers())
            {
                return false;
            }
            // TODO: Link somehow
            return false;
        }
    }
}
