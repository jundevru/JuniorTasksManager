using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Statics;

namespace TasksManagerClient.ViewModel
{
    class ChatViewModel : Helpers.Notifier, IPageDialog, IUpdateSenderReceiver
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

        public event Action ReceiveUpdate;

        public ICommand SendMessageCommand => new Helpers.CommandsDelegate((obj) => 
        {
            if (string.IsNullOrEmpty(Message))
                return;
            throw new NotImplementedException("Не реализована отправка сообщений");
        },(obj)=> {return isConnected; });

        public ChatViewModel()
        {

        }

        public void Disconnect()
        {

        }

        public void UpdatePropertyes()
        {
            //
        }

        public void SendUpdate()
        {
            // Отправить обновление в сеть
            MessageBox.Show("Отправляем обновление в сеть");
        }
        public void ReceivedUpdate()
        {
            // Принято обновление из сети
            Application.Current.Dispatcher.Invoke(() => {
                ReceiveUpdate?.Invoke();
                MessageBox.Show("Получено обновление из сети");
            });
        }
    }
}
