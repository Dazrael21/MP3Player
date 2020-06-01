using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Big_Finish_Player.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Property Changed Event Handler
        public void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Property Changed Event Handler
    }
}
