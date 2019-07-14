﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TasksManagerClient.Helpers
{
    class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }    
    }
}
