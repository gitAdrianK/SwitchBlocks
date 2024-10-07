using System;

namespace SwitchBlocks.Util
{
    [Serializable]
    public class BlockGroup
    {
        public bool State { get; set; }
        public float Progress { get; set; }
        public int ActivatedTick { get; set; }

        public BlockGroup(bool isEnabled)
        {
            State = isEnabled;
            Progress = isEnabled ? 1.0f : 0.0f;
            ActivatedTick = isEnabled ? Int32.MaxValue : Int32.MinValue;
        }
    }
}
