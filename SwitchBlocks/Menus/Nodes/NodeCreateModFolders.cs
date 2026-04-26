namespace SwitchBlocks.Menus
{
    using System.IO;
    using BehaviorTree;
    using JumpKing;
    using Patches;
    using Setups;

    /// <summary>
    ///     A BtNode responsible for creating the mods folder structure and the blocks.xml.
    /// </summary>
    public class NodeCreateModFolders : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating mod folders.");

            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The path '{directoryBin}' did not follow Worldsmith structure.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryMod = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder);
            Directory.CreateDirectory(directoryMod);
            // Audio.
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Audio));
            // Block types.
            if (SetupAuto.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Auto));
            }

            if (SetupBasic.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Basic));
            }

            if (SetupCountdown.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Countdown));
            }

            if (SetupGroup.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Group));
            }

            if (SetupJump.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Jump));
            }

            if (SetupSand.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Sand));
            }

            if (SetupSequence.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Sequence));
            }

            if (SetupThreshold.IsUsed)
            {
                Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Threshold));
            }

            // Textures.
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Textures));
            // Saves
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Saves));

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished creating mod folders.");
            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }
    }
}
