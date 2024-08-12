using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering basic platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityBasicPlatforms : EntityPlatforms
    {
        private static EntityBasicPlatforms instance;
        public static EntityBasicPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityBasicPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            DataBasic.Progress = progress;
            instance = null;
        }

        private EntityBasicPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.BASIC);
            progress = DataBasic.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataBasic.State, deltaTime, ModBlocks.BasicMultiplier);
        }
    }
}
