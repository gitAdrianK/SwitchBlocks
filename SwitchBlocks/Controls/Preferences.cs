namespace SwitchBlocks.Controls
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;
    using JetBrains.Annotations;
    using Microsoft.Xna.Framework.Input;

    public sealed class Preferences : INotifyPropertyChanged
    {
        private bool isEnabled;

        private Dictionary<EBinding, int[]> keyBinds = new Dictionary<EBinding, int[]>
        {
            { EBinding.Switch, new[] { (int)Keys.LeftControl } },
        };

        [XmlIgnore]
        public Dictionary<EBinding, int[]> KeyBindings
        {
            get => this.keyBinds;
            set
            {
                this.keyBinds = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get => this.isEnabled;
            set
            {
                this.isEnabled = value;
                this.OnPropertyChanged();
            }
        }

        public Binding[] Bindings
        {
            get => this.keyBinds.Select(kvp => new Binding(kvp)).ToArray();
            set => this.keyBinds = value.ToDictionary(x => x.Bind, x => x.Keys);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ForceUpdate() => this.OnPropertyChanged();

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public struct Binding
    {
        public Binding(KeyValuePair<EBinding, int[]> kvp) : this(kvp.Key, kvp.Value) { }

        public Binding(EBinding keyName, int[] actualKeys)
        {
            this.Bind = keyName;
            this.Keys = actualKeys;
        }

        // Without set the XmlSerializer cannot write binding properly
        [UsedImplicitly] public EBinding Bind { get; set; }

        [UsedImplicitly] public int[] Keys { get; set; }
    }
}
