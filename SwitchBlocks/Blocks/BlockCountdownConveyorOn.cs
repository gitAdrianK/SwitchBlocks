namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The countdown conveyor on block.
    /// </summary>
    public class BlockCountdownConveyorOn : ModBlock, IConveyor
    {
        /// <summary>Speed of the conveyor.</summary>
        private readonly float conveyorSpeed;

        /// <inheritdoc />
        public BlockCountdownConveyorOn(Rectangle collider, int speed) : base(collider) => this.conveyorSpeed = speed;

        /// <inheritdoc />
        public override Color DebugColor =>
            DataCountdown.Instance.State ? ModBlocks.CountdownConveyorOff : ModBlocks.CountdownConveyorOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public float Speed => this.conveyorSpeed * (DataCountdown.Instance.State ? 0.2f : -0.2f);
    }
}
