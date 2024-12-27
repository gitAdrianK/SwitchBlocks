namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

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

        public void Reset() => instance = null;

        private EntitySandLevers() => this.LeverDictionary = Lever.GetLeversDictonary(ModStrings.SAND);

        protected override void Update(float deltaTime)
        {
            if (this.UpdateCurrentScreen())
            {
                this.State = DataSand.State;
            }
        }
    }
}
