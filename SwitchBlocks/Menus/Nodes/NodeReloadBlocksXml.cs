namespace SwitchBlocks.Menus
{
    using System.IO;
    using System.Xml.Linq;
    using BehaviorTree;
    using JumpKing;
    using JumpKing.Player;
    using Settings;
    using Setups;

    public class NodeReloadBlocksXml : IBTnode
    {
        protected override BTresult MyRun(TickData tickData)
        {
            if (!ModDebug.IsDebug)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryMod = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder);
            if (!Directory.Exists(directoryMod))
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var file = Path.Combine(directoryMod, "blocks.xml");
            if (!File.Exists(file))
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var doc = XDocument.Load(file);
            var root = doc.Root;

            var debugInstance = ModDebug.Instance;

            if (SetupAuto.IsUsed)
            {
                // These should never be null at this point, but better safe than sorry.
                debugInstance.EntityLogicAuto?.UpdateSettings(new SettingsAuto(root?.Element("Auto")));
            }

            if (SetupBasic.IsUsed)
            {
                var settingsBasic = new SettingsBasic(root?.Element("Basic"));
                debugInstance.EntityLogicBasic?.UpdateSettings(settingsBasic);
                debugInstance.BehaviourBasicLever?.UpdateDirections(settingsBasic.LeverDirections);
            }

            if (SetupCountdown.IsUsed)
            {
                var settingsCountdown = new SettingsCountdown(root?.Element("Countdown"));
                debugInstance.EntityLogicCountdown?.UpdateSettings(settingsCountdown);
                debugInstance.BehaviourCountdownLever?.UpdateDirections(settingsCountdown.LeverDirections);
                debugInstance.BehaviourCountdownSingleUse?.UpdateDirections(settingsCountdown.LeverDirections);
            }

            if (SetupGroup.IsUsed)
            {
                var settingsGroup = new SettingsGroup(root?.Element("Group"));
                debugInstance.EntityLogicGroup?.UpdateSettings(settingsGroup);
                debugInstance.BehaviourGroupReset?.UpdateDirections(settingsGroup.LeverDirections);
            }

            if (SetupJump.IsUsed)
            {
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

            if (SetupSand.IsUsed)
            {
                var settingsSand = new SettingsSand(root?.Element("Sand"));
                debugInstance.EntityLogicSand?.UpdateSettings(settingsSand);
                debugInstance.BehaviourSandLever?.UpdateDirections(settingsSand.LeverDirections);
            }

            // ReSharper disable once InvertIf
            if (SetupSequence.IsUsed)
            {
                var settingsSequence = new SettingsSequence(root?.Element("Sequence"));
                debugInstance.EntityLogicSequence?.UpdateSettings(settingsSequence);
                debugInstance.BehaviourSequenceReset?.UpdateDirections(settingsSequence.LeverDirections);
                debugInstance.BehaviourSequenceReset?.UpdateDefaultActive(settingsSequence.DefaultActive);
            }

            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }
    }
}
