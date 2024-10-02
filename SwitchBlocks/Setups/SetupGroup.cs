using EntityComponent;
using JumpKing.API;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using SwitchBlocks.Behaviours;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SwitchBlocks.Setups
{
    public static class SetupGroup
    {
        // The Groups cannot be reset on start or end as the factory only runs when a new level is loaded
        // clearing would result in the dict being empty on same level reload.
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupA { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();
        public static Dictionary<Vector3, IBlockGroupId> BlocksGroupB { get; private set; } = new Dictionary<Vector3, IBlockGroupId>();

        public static void DoSetup(PlayerEntity player)
        {
            if (!ModBlocks.IsGroupUsed)
            {
                return;
            }

            _ = DataGroup.Instance;

            AssignGroupIds();
            CreateGroupData();

            _ = EntityGroupPlatforms.Instance;

            IBlockBehaviour behaviourGroupPlatform;
            if (ModBlocks.GroupDuration == 0)
            {
                behaviourGroupPlatform = new BehaviourGroupLeaving();
            }
            else
            {
                //TODO : Duration behaviour
                behaviourGroupPlatform = new BehaviourGroupDuration();
            }
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupA), behaviourGroupPlatform);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupB), behaviourGroupPlatform);

            BehaviourGroupIceA behaviourGroupIceA = new BehaviourGroupIceA();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceA), behaviourGroupIceA);
            BehaviourGroupIceB behaviourGroupIceB = new BehaviourGroupIceB();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupIceB), behaviourGroupIceB);

            BehaviourGroupSnow behaviourGroupSnow = new BehaviourGroupSnow();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowA), behaviourGroupSnow);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupSnowB), behaviourGroupSnow);

            BehaviourGroupReset behaviourGroupReset = new BehaviourGroupReset();
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupReset), behaviourGroupReset);
            player.m_body.RegisterBlockBehaviour(typeof(BlockGroupResetSolid), behaviourGroupReset);
        }

        public static void DoCleanup(EntityManager entityManager)
        {
            if (!ModBlocks.IsGroupUsed)
            {
                return;
            }

            entityManager.RemoveObject(EntityGroupPlatforms.Instance);
            EntityGroupPlatforms.Instance.Reset();

            DataGroup.Instance.SaveToFile();
            DataGroup.Instance.Reset();
        }

        // Group ID starts at 1 since 0 is used for "not in any group"
        private static int groupId = 1;

        private static void AssignGroupIds()
        {
            // Sort for consistency, that is that I don't have to worry about
            // the group ids getting potentially messed up.
            // I kinda do want to do caching at some point but for now
            // keeping it simple, albeit inefficient.
            BlocksGroupA = BlocksGroupA.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
            BlocksGroupB = BlocksGroupB.OrderBy(kv => kv.Key.Z)
                .ThenBy(kv => kv.Key.X)
                .ThenBy(kv => kv.Key.Y)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            groupId = 1;
            if (DataGroup.Groups.Count() > 0)
            {
                groupId = DataGroup.Groups.OrderByDescending(kv => kv.Key).First().Key + 1;
            }

            AssignGroupIds(BlocksGroupA);
            AssignGroupIds(BlocksGroupB);
        }

        /// <summary>
        /// Groups up all blocks next to eachother into a group by assigning them the same ID.
        /// </summary>
        /// <param name="blocks">The coordinates and blocks that are to be grouped</param>
        private static void AssignGroupIds(Dictionary<Vector3, IBlockGroupId> blocks)
        {
            Queue<Vector3> toVisit = new Queue<Vector3>();
            HashSet<Vector3> visited = new HashSet<Vector3>();
            foreach (KeyValuePair<Vector3, IBlockGroupId> kv in blocks)
            {
                Vector3 position = kv.Key;
                IBlockGroupId block = kv.Value;
                if (visited.Contains(position) || block.GroupId != 0)
                {
                    continue;
                }
                toVisit.Enqueue(position);
                while (toVisit.Count != 0)
                {
                    Vector3 currentPos = toVisit.Dequeue();
                    blocks[currentPos].GroupId = groupId;

                    visited.Add(currentPos);
                    // Left
                    Vector3 left = currentPos + new Vector3(-1, 0, 0);
                    if (!visited.Contains(left) && blocks.ContainsKey(left))
                    {
                        toVisit.Enqueue(left);
                    }
                    // Right
                    Vector3 right = currentPos + new Vector3(1, 0, 0);
                    if (!visited.Contains(right) && blocks.ContainsKey(right))
                    {
                        toVisit.Enqueue(right);
                    }
                    // Up
                    Vector3 up = currentPos + new Vector3(0, -1, 0);
                    if (up.Y == -1)
                    {
                        up = new Vector3(currentPos.X, 44, currentPos.Z + 1);
                    }
                    if (!visited.Contains(up) && blocks.ContainsKey(up))
                    {
                        toVisit.Enqueue(up);
                    }
                    // Down
                    Vector3 down = currentPos + new Vector3(0, 1, 0);
                    if (down.Y == 45)
                    {
                        down = new Vector3(currentPos.X, 0, currentPos.Z - 1);
                    }
                    if (!visited.Contains(down) && blocks.ContainsKey(down))
                    {
                        toVisit.Enqueue(down);
                    }
                }
                groupId++;
            }
        }

        private static void CreateGroupData()
        {
            for (int i = 1; i < groupId; i++)
            {
                if (!DataGroup.Groups.ContainsKey(i))
                {
                    DataGroup.Groups.Add(i, new DataGroup.Group());
                }
            }
        }
    }
}