namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The jump conveyor off block.
    /// </summary>
    public class BlockJumpConveyorOff : ModBlock, IConveyor
    {
        /// <summary>Speed of the conveyor.</summary>
        private readonly float conveyorSpeed;

        /// <inheritdoc />
        public BlockJumpConveyorOff(Rectangle collider, int speed) : base(collider) => this.conveyorSpeed = speed;

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataJump.Instance.State ? ModBlocks.JumpConveyorOff : ModBlocks.JumpConveyorOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public float Speed => this.conveyorSpeed * (!DataJump.Instance.State ? 0.2f : -0.2f);
    }
}
