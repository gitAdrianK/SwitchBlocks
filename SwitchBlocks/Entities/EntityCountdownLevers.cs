namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

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

        public void Reset() => instance = null;

        private EntityCountdownLevers() => this.LeverDictionary = Lever.GetLeversDictonary(ModStrings.COUNTDOWN);

        protected override void Update(float deltaTime)
        {
            if (this.UpdateCurrentScreen())
            {
                this.State = DataCountdown.State;
            }
        }
    }
}
