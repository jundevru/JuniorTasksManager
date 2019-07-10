using System;
using ChatTestApp.Helpers;
using NetworkCore;
using System.Windows;
using System.Collections.ObjectModel;

namespace ChatTestApp.ViewModels
{
    class MainWindowViewModel : Notifier
    {
        private Client client;


        private string ip;
        public string IP
        {
            get { return ip; }
            set
            {
                ip = value;
                NotifyPropertyChanged("IP");
            }
        }

        private string nick;
        public string Nick
        {
            get { return nick; }
            set
            {
                nick = value;
                NotifyPropertyChanged("Nick");
            }
        }

        private bool auth;
        public bool Auth
        {
            get { return auth; }
            set
            {
                auth = value;
                NotifyPropertyChanged("Auth");
            }
        }

        private ObservableCollection<Model.ChatMessageModel> messages;
        public ObservableCollection<Model.ChatMessageModel> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                NotifyPropertyChanged("Messages");
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }

        public CommandDelegate ConnectCommand =>
        new CommandDelegate((obj) =>
        {
            client = new Client(IP, NetworkCore.Utilits.DefaultPort,
                (res) =>
                {
                    InvokeHelper.InvokeEx(() =>
                    {
                        Auth = res;
                        if (!res)
                            client.Disconnect();
                    });
                },
                (recivedObject) => {
                    InvokeHelper.InvokeEx(() => {
                        Messages.Add(recivedObject as Model.ChatMessageModel);
                    });
                }, null,
                () =>
                {
                    InvokeHelper.InvokeEx(() =>
                    {
                        Auth = false;
                    });
                },
                (message) =>
                {
                    InvokeHelper.InvokeEx(() =>
                    {
                        MessageBox.Show(message);
                    });
                });
            if (client.Connect())
                client.SendAuth(Nick);
        }, (obj) =>
        {
            if (client == null || !client.IsConnected) return true;
            return false;
        });

        public CommandDelegate SendCommand =>
            new CommandDelegate((obj) =>
            {
                if (!String.IsNullOrEmpty(Message))
                {
                    client.SendObject(new Model.ChatMessageModel(Nick + ": " + Message));
                    Message = "";
                }
            }, (obj) =>
            {
                return Auth;
            });

        public MainWindowViewModel()
        {
            IP = "127.0.0.1";
            Nick = "User" + DateTime.Now.ToShortTimeString();
            Messages = new ObservableCollection<Model.ChatMessageModel>();
        }
    }
}
