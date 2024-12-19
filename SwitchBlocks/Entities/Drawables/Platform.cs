namespace SwitchBlocks.Entities.Drawables
{
    using System.Xml.Serialization;

    public abstract class Platform : Drawable
    {
        [XmlIgnore]
        protected bool StartState { get; set; }
        [XmlElement("StartState")]
        public string StartStateAsString
        {
            get => this.StartState.ToString();
            set => this.StartState = value.ToLower().Equals("on");
        }
    }
}
