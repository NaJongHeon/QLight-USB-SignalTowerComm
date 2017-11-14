using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SignalTowerComm.ViewModel.Base
{
    public class CustomViewModelBase : ViewModelBase
    {
        public string ViewModelGuid
        {
            get { return _viewModelGuid; }
            set { _viewModelGuid = value; }
        }
        private string _viewModelGuid;

        public Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; RaisePropertyChanged("Visibility"); }
        }
        private Visibility _visibility = Visibility.Collapsed;
    }
}
