using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;

namespace SwitchBlocksMod.Entities
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
            UpdateProgress(DataJump.State, deltaTime, ModBlocks.jumpMultiplier);
        }
    }
}
