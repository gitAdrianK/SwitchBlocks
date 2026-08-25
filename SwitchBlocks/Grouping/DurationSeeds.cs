namespace SwitchBlocks.Grouping
{
    using System.Collections.Generic;

    /// <summary>
    ///     Special dictionary to hold the "position to duration" seeds.
    /// </summary>
    public class DurationSeeds : Dictionary<int, int>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public DurationSeeds() { }

        /// <summary>
        ///     Ctor taking a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public DurationSeeds(IDictionary<int, int> dictionary) : base(dictionary) { }

        /// <summary>
        ///     Positions of all seeds.
        /// </summary>
        public KeyCollection Positions => this.Keys;

        /// <summary>
        ///     Ids of all seeds.
        /// </summary>
        public ValueCollection Durations => this.Values;
    }
}
