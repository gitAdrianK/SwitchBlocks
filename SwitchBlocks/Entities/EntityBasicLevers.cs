using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering basic levers in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityBasicLevers : EntityLevers
    {
        private static EntityBasicLevers instance;
        public static EntityBasicLevers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityBasicLevers();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private EntityBasicLevers()
        {
            LeverDictionary = Lever.GetLeversDictonary(ModStrings.BASIC);
        }

        protected override void Update(float deltaTime)
        {
            if (UpdateCurrentScreen())
            {
                state = DataBasic.State;
            }
        }
    }
}
