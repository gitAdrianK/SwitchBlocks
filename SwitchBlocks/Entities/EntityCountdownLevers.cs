using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering countdown levers in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityCountdownLevers : EntityLevers
    {
        private static EntityCountdownLevers instance;
        public static EntityCountdownLevers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityCountdownLevers();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private EntityCountdownLevers()
        {
            LeverDictionary = Lever.GetLeversDictonary(ModStrings.COUNTDOWN);
        }

        protected override void Update(float deltaTime)
        {
            if (UpdateCurrentScreen())
            {
                state = DataCountdown.State;
            }
        }
    }
}
