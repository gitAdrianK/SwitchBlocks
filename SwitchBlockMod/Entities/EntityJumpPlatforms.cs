using SwitchBlocksMod.Data;

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
            instance = null;
        }

        private EntityJumpPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("jump");
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataJump.State, deltaTime);
        }
    }
}
