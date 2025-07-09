namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Settings;
    using Setups;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockSequenceA" />.
    /// </summary>
    public class BehaviourSequencePlatform : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourSequencePlatform()
        {
            var data = DataSequence.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
        }

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active.</summary>
        private HashSet<int> Active { get; }

        /// <summary>ID considered touched.</summary>
        private static int Touched
        {
            get => DataSequence.Instance.Touched;
            set => DataSequence.Instance.Touched = value;
        }

        /// <inheritdoc />
        public float BlockPriority => ModConstants.PrioNormal;

        /// <inheritdoc />
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc />
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc />
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo == null)
            {
                return true;
            }

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockSequenceA>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceB>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceC>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceD>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceA>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceB>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceC>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceD>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowA>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowB>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowC>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowD>();
            if (!this.IsPlayerOnBlock)
            {
                var touched = Touched;
                if (!SettingsSequence.DisableOnLeave
                    || SettingsSequence.Duration != 0
                    || touched <= 0)
                {
                    return true;
                }

                if (!this.Groups.TryGetValue(touched, out var group))
                {
                    return true;
                }

                group.ActivatedTick = int.MinValue;
                _ = this.Active.Add(touched);

                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockSequenceA)
                       || type == typeof(BlockSequenceB)
                       || type == typeof(BlockSequenceC)
                       || type == typeof(BlockSequenceD)
                       || type == typeof(BlockSequenceIceA)
                       || type == typeof(BlockSequenceIceB)
                       || type == typeof(BlockSequenceIceC)
                       || type == typeof(BlockSequenceIceD)
                       || type == typeof(BlockSequenceSnowA)
                       || type == typeof(BlockSequenceSnowB)
                       || type == typeof(BlockSequenceSnowC)
                       || type == typeof(BlockSequenceSnowD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                var groupId = block.GroupId;
                if (!this.Groups.TryGetValue(groupId, out var group))
                {
                    continue;
                }

                if (!group.State
                    || Touched != groupId - 1
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsSequence.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }

                if (SettingsSequence.Duration == 0)
                {
                    if (groupId > 1)
                    {
                        if (!this.Groups.TryGetValue(groupId - 1, out var prevGroup))
                        {
                            continue;
                        }

                        prevGroup.ActivatedTick = int.MinValue;
                        _ = this.Active.Add(groupId - 1);
                    }
                }
                else
                {
                    var tick = PatchAchievementManager.GetTick();
                    group.ActivatedTick = tick + SettingsSequence.Duration;
                    _ = this.Active.Add(groupId);
                }

                if (groupId < SetupSequence.SequenceCount)
                {
                    if (!this.Groups.TryGetValue(groupId + 1, out var nextGroup))
                    {
                        continue;
                    }

                    nextGroup.ActivatedTick = int.MaxValue;
                    _ = this.Active.Add(groupId + 1);
                }

                Touched = groupId;
                break;
            }

            return true;
        }
    }
}
