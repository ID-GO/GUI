using System.ComponentModel;
using System.Runtime.CompilerServices;
using GUI.Annotations;

namespace GUI.Model
{
    public class Basepropertychangedevent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}