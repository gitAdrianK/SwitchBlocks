namespace SwitchBlocks.Util
{
    public class WrappedIndex
    {
        public WrappedIndex(int length) => this.Length = length;

        public int Index
        {
            get => this.InternalIndex;
            set
            {
                this.InternalIndex = value;
                while (this.InternalIndex < 0)
                {
                    this.InternalIndex += this.Length;
                }
                while (this.InternalIndex >= this.Length)
                {
                    this.InternalIndex -= this.Length;
                }
            }
        }

        private int Length { get; }
        private int InternalIndex { get; set; }
    }
}
