namespace SwitchBlocks.Grouping
{
    using System.Collections.Generic;

    /// <summary>
    ///     Special dictionary to hold the "position to id" seeds.
    /// </summary>
    public class IdSeedsDictionary : Dictionary<int, int>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public IdSeedsDictionary() { }

        /// <summary>
        ///     Ctor taking a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public IdSeedsDictionary(IDictionary<int, int> dictionary) : base(dictionary) { }

        /// <summary>
        ///     Positions of all seeds.
        /// </summary>
        public KeyCollection Positions => this.Keys;

        /// <summary>
        ///     Ids of all seeds.
        /// </summary>
        public ValueCollection Ids => this.Values;
    }
}
