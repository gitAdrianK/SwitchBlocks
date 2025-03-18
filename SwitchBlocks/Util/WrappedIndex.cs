namespace SwitchBlocks.Util
{
    public class WrappedIndex
    {
        public WrappedIndex(int length) => this.Length = length;

        public int Index
        {
            get => this.Index;
            set
            {
                this.Index = value;
                while (this.Index < 0)
                {
                    this.Index += this.Length;
                }
                while (this.Index >= this.Length)
                {
                    this.Index -= this.Length;
                }
            }
        }

        private int Length { get; }
    }
}
