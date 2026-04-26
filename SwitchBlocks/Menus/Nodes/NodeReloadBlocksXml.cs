namespace SwitchBlocks.Menus
{
    using System.IO;
    using System.Xml.Linq;
    using BehaviorTree;
    using JumpKing;
    using JumpKing.Player;
    using Patches;
    using Settings;
    using Setups;

    /// <summary>
    ///     A BtNode responsible for reloading the blocks.xml.
    /// </summary>
    public class NodeReloadBlocksXml : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Reloading settings.");

            if (!ModDebug.IsDebug)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The path '{directoryBin}' did not follow Worldsmith structure.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryMod = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder);
            if (!Directory.Exists(directoryMod))
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The path '{directoryMod}' did not exist.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var file = Path.Combine(directoryMod, "blocks.xml");
            if (!File.Exists(file))
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The file '{file}' did not exist.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var doc = XDocument.Load(file);
            var root = doc.Root;

            var debugInstance = ModDebug.Instance;

            this.ReloadAutoSettings(root, debugInstance);
            this.ReloadBasicSettings(root, debugInstance);
            this.ReloadCountdownSettings(root, debugInstance);
            this.ReloadGroupSettings(root, debugInstance);
            this.ReloadJumpSettings(root, debugInstance);
            this.ReloadSandSettings(root, debugInstance);
            this.ReloadSequenceSettings(root, debugInstance);
            this.ReloadThresholdSettings(root, debugInstance);

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished reloading settings.");
            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }

        /// <summary>
        ///     Reload settings of the auto type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadAutoSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupAuto.IsUsed)
            {
                return;
            }

            // These should never be null at this point, but better safe than sorry.
            var settingsAuto = new SettingsAuto(root?.Element("Auto"));
            debugInstance.EntityLogicAuto?.UpdateSettings(settingsAuto);
            debugInstance.BehaviourAutoReset?.UpdateDuration(settingsAuto.DurationOff);
        }

        /// <summary>
        ///     Reload settings of the basic type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadBasicSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupBasic.IsUsed)
            {
                return;
            }

            var settingsBasic = new SettingsBasic(root?.Element("Basic"));
            debugInstance.EntityLogicBasic?.UpdateSettings(settingsBasic);
            debugInstance.BehaviourBasicLever?.UpdateDirections(settingsBasic.LeverDirections);
        }

        /// <summary>
        ///     Reload settings of the countdown type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadCountdownSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupCountdown.IsUsed)
            {
                return;
            }

            var settingsCountdown = new SettingsCountdown(root?.Element("Countdown"));
            debugInstance.EntityLogicCountdown?.UpdateSettings(settingsCountdown);
            debugInstance.BehaviourCountdownLever?.UpdateDirections(settingsCountdown.LeverDirections);
            debugInstance.BehaviourCountdownSingleUse?.UpdateDirections(settingsCountdown.LeverDirections);
        }

        /// <summary>
        ///     Reload settings of the group type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadGroupSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupGroup.IsUsed)
            {
                return;
            }

            var settingsGroup = new SettingsGroup(root?.Element("Group"));
            debugInstance.EntityLogicGroup?.UpdateSettings(settingsGroup);
            debugInstance.BehaviourGroupReset?.UpdateDirections(settingsGroup.LeverDirections);
        }

        /// <summary>
        ///     Reload settings of the jump type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadJumpSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupJump.IsUsed)
            {
                return;
            }

            var settingsJump = new SettingsJump(root?.Element("Jump"));
            debugInstance.EntityLogicJump?.UpdateSettings(settingsJump);

            PlayerEntity.OnJumpCall -= SetupJump.JumpSwitchSafe;
            PlayerEntity.OnJumpCall -= SetupJump.JumpSwitchUnsafe;
            if (!settingsJump.ForceSwitch)
            {
                PlayerEntity.OnJumpCall += SetupJump.JumpSwitchSafe;
            }
            else
            {
                PlayerEntity.OnJumpCall += SetupJump.JumpSwitchUnsafe;
            }
        }

        /// <summary>
        ///     Reload settings of the sand type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadSandSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupSand.IsUsed)
            {
                return;
            }

            var settingsSand = new SettingsSand(root?.Element("Sand"));
            debugInstance.EntityLogicSand?.UpdateSettings(settingsSand);
            debugInstance.BehaviourSandLever?.UpdateDirections(settingsSand.LeverDirections);
        }

        /// <summary>
        ///     Reload settings of the sequence type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadSequenceSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupSequence.IsUsed)
            {
                return;
            }

            var settingsSequence = new SettingsSequence(root?.Element("Sequence"));
            debugInstance.EntityLogicSequence?.UpdateSettings(settingsSequence);
            debugInstance.BehaviourSequenceReset?.UpdateDirections(settingsSequence.LeverDirections);
            debugInstance.BehaviourSequenceReset?.UpdateDefaultActive(settingsSequence.DefaultActive);
        }

        /// <summary>
        ///     Reload settings of the threshold type block.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> that might contain settings.</param>
        /// <param name="debugInstance">Instance of the debug class holding relevant fields.</param>
        private void ReloadThresholdSettings(XElement root, ModDebug debugInstance)
        {
            if (!SetupThreshold.IsUsed)
            {
                return;
            }

            var settingsThreshold = new SettingsThreshold(root?.Element("Threshold"));
            debugInstance.EntityLogicThreshold?.UpdateSettings(settingsThreshold);
            debugInstance.BehaviourThresholdReset?.UpdateStat(settingsThreshold.Stat);
        }
    }
}
