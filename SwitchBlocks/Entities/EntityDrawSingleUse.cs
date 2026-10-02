namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Microsoft.Xna.Framework;
    using Util.Deserialization;

    /// <summary>
    ///     Single use lever drawn based on data.
    /// </summary>
    public class EntityDrawSingleUse : EntityDrawLever
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="lever">Deserialization helper <see cref="Lever" />.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        /// <param name="touched">Ids that have been touched.</param>
        /// <param name="id">The ID of the single-use lever.</param>
        public EntityDrawSingleUse(
            Lever lever,
            int screen,
            IDataProvider data,
            HashSet<int> touched,
            int id)
            : base(lever, screen, data)
        {
            this.Id = id;
            this.Touched = touched;
        }

        /// <summary>Lever id this drawable is tied to.</summary>
        private int Id { get; }

        /// <summary>Ids that have been touched.</summary>
        private HashSet<int> Touched { get; }

        /// <summary>
        ///     Draws the entity if the current screen is the screen it appears on or the game has not finished yet.
        ///     Based on state given by the <see cref="IDataProvider" /> the left of right half of the texture is drawn.
        /// </summary>
        public override void Draw() =>
            this.DrawWithRectangle(new Rectangle(
                this.Width * Convert.ToInt32(!this.Touched.Contains(this.Id)),
                0,
                this.Width,
                this.Height)
            );
    }
}
