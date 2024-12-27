namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

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

        public void Reset() => instance = null;

        private EntityBasicLevers() => this.LeverDictionary = Lever.GetLeversDictonary(ModStrings.BASIC);

        protected override void Update(float deltaTime)
        {
            if (this.UpdateCurrentScreen())
            {
                this.State = DataBasic.State;
            }
        }
    }
}
