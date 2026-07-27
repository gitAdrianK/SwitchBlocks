namespace SwitchBlocks.Menus
{
    using System.IO;
    using BehaviorTree;
    using Data;
    using JumpKing;
    using Patches;
    using Setups;
    using Util;

    /// <summary>
    ///     A <see cref="IBTnode" /> responsible for reloading various seeds/resets/durations files.
    /// </summary>
    public class NodeReloadSeeds : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Reloading seeds.");

            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The path '{directoryBin}' did not follow Worldsmith structure.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directorySaves = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder, ModConstants.Saves);
            if (!Directory.Exists(directorySaves))
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The path '{directorySaves}' did not exist.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            this.ReloadSeedsAuto(directorySaves);
            this.ReloadSeedsCountdown(directorySaves);
            this.ReloadSeedsGroup(directorySaves);
            this.ReloadSeedsSequence(directorySaves);

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished reloading seeds.");
            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }

        private void ReloadSeedsAuto(string directorySaves)
        {
            if (!SetupAuto.IsUsed)
            {
                return;
            }

            foreach (var block in SetupAuto.ChangeDuration.Values)
            {
                block.Duration = BlockDuration.NotAssigned;
            }

            var seedsDuration = DurationsAuto.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixDurations}{ModConstants.Auto}{ModConstants.SuffixSav}"));
            SetupAuto.AssignByDuration(seedsDuration.Seeds);

            seedsDuration.SaveToFile();
        }

        private void ReloadSeedsCountdown(string directorySaves)
        {
            if (!SetupCountdown.IsUsed)
            {
                return;
            }

            foreach (var block in SetupCountdown.SingleUseLevers.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupCountdown.CustomDurationLevers.Values)
            {
                block.Duration = BlockDuration.NotAssigned;
            }

            var seedsId = SeedsCountdown.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixSeeds}{ModConstants.Countdown}{ModConstants.SuffixSav}"));
            SetupCountdown.AssignByGroups(seedsId.Seeds);

            var seedsDuration = DurationsCountdown.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixDurations}{ModConstants.Countdown}{ModConstants.SuffixSav}"));
            SetupCountdown.AssignByDuration(seedsDuration.Seeds);

            seedsId.SaveToFile();
            seedsDuration.SaveToFile();
        }

        private void ReloadSeedsGroup(string directorySaves)
        {
            if (!SetupGroup.IsUsed)
            {
                return;
            }

            foreach (var block in SetupGroup.BlocksGroupA.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupGroup.BlocksGroupB.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupGroup.BlocksGroupC.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupGroup.BlocksGroupD.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupGroup.Deactivates.Values)
            {
                block.Ids = new int[0];
            }

            foreach (var block in SetupGroup.Resets.Values)
            {
                block.Ids = new int[0];
            }

            var instance = DataGroup.Instance;
            instance.Groups.Clear();

            var seeds = SeedsGroup.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixSeeds}{ModConstants.Group}{ModConstants.SuffixSav}"));
            var resets = ResetsGroup.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixResets}{ModConstants.Group}{ModConstants.SuffixSav}"));
            var deactivates = DeactivatesGroup.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixDeactivates}{ModConstants.Group}{ModConstants.SuffixSav}"));
            SetupGroup.AssignGroupIds(DataGroup.Instance.Groups, seeds.Seeds, resets.Resets, deactivates.Deactivates);

            seeds.SaveToFile();
            resets.SaveToFile();
            deactivates.SaveToFile();
        }

        private void ReloadSeedsSequence(string directorySaves)
        {
            if (!SetupSequence.IsUsed)
            {
                return;
            }

            foreach (var block in SetupSequence.BlocksSequenceA.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupSequence.BlocksSequenceB.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupSequence.BlocksSequenceC.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupSequence.BlocksSequenceD.Values)
            {
                block.GroupId = BlockGroupId.NotAssigned;
            }

            foreach (var block in SetupSequence.Resets.Values)
            {
                block.Ids = new int[0];
            }

            var instance = DataSequence.Instance;
            instance.Groups.Clear();

            var seeds = SeedsSequence.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixSeeds}{ModConstants.Sequence}{ModConstants.SuffixSav}"));
            var resets = ResetsSequence.TryDeserialize(Path.Combine(directorySaves,
                $"{ModConstants.PrefixResets}{ModConstants.Sequence}{ModConstants.SuffixSav}"));

            SetupSequence.AssignSequenceIds(instance.Groups, seeds.Seeds, resets.Resets);

            seeds.SaveToFile();
            resets.SaveToFile();

            foreach (var defaultId in ModDebug.Instance.DefaultActiveSequence)
            {
                if (instance.Groups.TryGetValue(defaultId, out var group))
                {
                    group.ActivatedTick = int.MaxValue;
                }

                _ = instance.Active.Add(defaultId);
            }
        }
    }
}
