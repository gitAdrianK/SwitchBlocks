namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The auto conveyor off block.
    /// </summary>
    public class BlockAutoConveyorOff : ModBlock, IConveyor
    {
        /// <summary>Speed of the conveyor.</summary>
        private readonly float conveyorSpeed;

        /// <inheritdoc />
        public BlockAutoConveyorOff(Rectangle collider, int speed) : base(collider) => this.conveyorSpeed = speed;

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataAuto.Instance.State ? ModBlocks.AutoConveyorOff : ModBlocks.AutoConveyorOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public float Speed => this.conveyorSpeed * (!DataAuto.Instance.State ? 0.2f : -0.2f);
    }
}
