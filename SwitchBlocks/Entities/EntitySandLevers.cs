using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering sand levers in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntitySandLevers : EntityLevers
    {
        private static EntitySandLevers instance;
        public static EntitySandLevers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntitySandLevers();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private EntitySandLevers()
        {
            LeverDictionary = Lever.GetLeversDictonary(ModStrings.SAND);
        }

        protected override void Update(float deltaTime)
        {
            if (UpdateCurrentScreen())
            {
                state = DataSand.State;
            }
        }
    }
}
