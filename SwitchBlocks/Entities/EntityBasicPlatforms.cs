namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Platforms;
    using SwitchBlocks.Settings;

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
            DataBasic.Progress = this.Progress;
            instance = null;
        }

        private EntityBasicPlatforms()
        {
            this.PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.BASIC);
            this.Progress = DataBasic.Progress;
        }

        protected override void Update(float deltaTime)
            => this.UpdateProgress(DataBasic.State, deltaTime, SettingsBasic.Multiplier);

        public override void Draw()
        {
            if (!this.UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            var spriteBatch = Game1.spriteBatch;
            _ = Parallel.ForEach(this.CurrentPlatformList, platform
                => DrawPlatform(platform, this.Progress, DataBasic.State, spriteBatch));
        }
    }
}
