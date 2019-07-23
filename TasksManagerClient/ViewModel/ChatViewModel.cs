using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TasksManagerClient.Dialogs;

namespace TasksManagerClient.ViewModel
{
    class ChatViewModel : Helpers.Notifier, IPageDialog
    {
        public string Title => "Сообщения";

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged();
            }
        }
        private bool isConnected = false;

        public ICommand SendMessageCommand => new Helpers.CommandsDelegate((obj) => 
        {
            if (string.IsNullOrEmpty(Message))
                return;
            throw new NotImplementedException("Не реализована отправка сообщений");
        },(obj)=> {return isConnected; });

    }
}
