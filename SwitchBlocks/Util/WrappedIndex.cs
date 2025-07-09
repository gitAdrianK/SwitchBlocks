namespace SwitchBlocks.Util
{
    /// <summary>
    ///     Similar in function to the vanilla WrappedIndex, but it is <c>internal</c>
    ///     and this is easier than somehow getting it with Harmony.
    /// </summary>
    public class WrappedIndex
    {
        /// <summary>
        ///     WrappedIndex ctor.
        /// </summary>
        /// <param name="length">Length this index starts to wrap once reached.</param>
        public WrappedIndex(int length) => this.Length = length;

        /// <summary>The index.</summary>
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

        /// <summary>Length.</summary>
        private int Length { get; }

        /// <summary>Internal index.</summary>
        private int InternalIndex { get; set; }
    }
}
