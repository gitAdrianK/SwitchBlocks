namespace SwitchBlocks.Menus
{
    using System.IO;
    using BehaviorTree;
    using JumpKing;

    /// <summary>
    ///     A BtNode responsible for creating the mods folder structure and the blocks.xml.
    /// </summary>
    public class NodeCreateModFolders : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryMod = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder);
            Directory.CreateDirectory(directoryMod);
            // Audio.
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Audio));
            // Block types.
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Auto));
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Basic));
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Countdown));
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Group));
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Jump));
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Sand));
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Sequence));
            // Textures.
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Textures));
            // Saves
            Directory.CreateDirectory(Path.Combine(directoryMod, ModConstants.Saves));

            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }
    }
}
