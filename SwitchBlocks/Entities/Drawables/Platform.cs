using System.Xml.Serialization;

namespace SwitchBlocks.Entities.Drawables
{
    public abstract class Platform : Drawable
    {
        [XmlIgnore]
        protected bool StartState { get; set; }
        [XmlElement("StartState")]
        public string StartStateAsString
        {
            get
            {
                return StartState.ToString();
            }
            set
            {
                StartState = value.ToLower().Equals("on");
            }
        }
    }
}
