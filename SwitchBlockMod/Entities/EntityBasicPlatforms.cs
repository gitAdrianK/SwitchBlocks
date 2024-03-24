using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Entities
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
            instance = null;
        }

        private EntityBasicPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("basic");
        }

        protected override void Update(float deltaTime)
        {
            if (!UpdateCurrentScreen())
            {
                return;
            }

            if (progress != 1.0f && DataBasic.State)
            {
                progress += deltaTime;
                if (progress >= 1.0f)
                {
                    progress = 1.0f;
                }
            }
            else if (progress != 0.0f && !DataBasic.State)
            {
                progress -= 5.0f * deltaTime;
                if (progress <= 0.0f)
                {
                    progress = 0.0f;
                }
            }
        }
    }
}

