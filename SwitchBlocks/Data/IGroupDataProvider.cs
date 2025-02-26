namespace SwitchBlocks.Data
{
    public interface IGroupDataProvider
    {
        bool GetState(int id);
        float GetProgress(int id);
    }
}
