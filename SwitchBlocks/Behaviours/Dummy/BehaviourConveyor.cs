namespace SwitchBlocks.Behaviours.Dummy
{
    using System.Linq;
    using Blocks;
    using Blocks.Dummy;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockConveyor" />.
    /// </summary>
    public class BehaviourConveyor : IBlockBehaviour
    {
        ///<summary>If the player is on any conveyor block.</summary>
        public static bool IsPlayerOnConveyor { get; set; }

        ///<summary>If the player was on any conveyor block last frame.</summary>
        public static bool WasPlayerOnConveyor { get; set; }

        /// <summary>First collided conveyor block.</summary>
        public static IBlock ConveyorBlock { get; set; }

        /// <summary>The position of the previous time the player has interacted with a conveyor.</summary>
        public static Vector2 ConveyorPrevPosition { get; private set; } = new Vector2(0, 0);

        /// <inheritdoc />
        public float BlockPriority => ModConstants.PrioLate;

        /// <inheritdoc />
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc />
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            if (!this.IsPlayerOnBlock || !bodyComp.IsOnGround)
            {
                return inputXVelocity;
            }

            var info = behaviourContext.LastFrameCollisionInfo?.PreResolutionCollisionInfo;
            if (info == null)
            {
                return inputXVelocity;
            }

            ConveyorBlock = info.GetCollidedBlocks<BlockBasicConveyorOn>().FirstOrDefault()
                            ?? info.GetCollidedBlocks<BlockBasicConveyorOff>().FirstOrDefault()
                            ?? info.GetCollidedBlocks<BlockCountdownConveyorOn>().FirstOrDefault()
                            ?? info.GetCollidedBlocks<BlockCountdownConveyorOff>().FirstOrDefault();
            WasPlayerOnConveyor = this.IsPlayerOnBlock;
            ConveyorPrevPosition = bodyComp.Position;

            var conveyor = (IConveyor)ConveyorBlock;
            return inputXVelocity + conveyor.Speed;
        }

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc />
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var info = behaviourContext.LastFrameCollisionInfo?.PreResolutionCollisionInfo;
            if (info == null)
            {
                return true;
            }

            this.IsPlayerOnBlock = info.IsCollidingWith<BlockBasicConveyorOn>()
                                   || info.IsCollidingWith<BlockBasicConveyorOff>()
                                   || info.IsCollidingWith<BlockCountdownConveyorOn>()
                                   || info.IsCollidingWith<BlockCountdownConveyorOff>();
            IsPlayerOnConveyor = this.IsPlayerOnBlock;
            return true;
        }
    }
}
