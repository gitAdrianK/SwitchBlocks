using SwitchBlocks.Data;
using SwitchBlocks.Platforms;

namespace SwitchBlocks.Entities
{
    public class EntityJumpPlatforms : EntityPlatforms
    {
        private static EntityJumpPlatforms instance;
        public static EntityJumpPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityJumpPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            DataJump.Progress = progress;
            instance = null;
        }

        private EntityJumpPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.JUMP);
            progress = DataJump.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataJump.State, deltaTime, ModBlocks.JumpMultiplier);
        }
    }
}
