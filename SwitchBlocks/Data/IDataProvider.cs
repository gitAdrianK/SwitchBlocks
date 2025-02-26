namespace SwitchBlocks.Data
{
    public interface IDataProvider
    {
        bool State { get; set; }
        float Progress { get; set; }
    }
}
