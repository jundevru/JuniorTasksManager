using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatTestApp.Model
{
    [Serializable]
    class ChatMessageModel : Helpers.Notifier, NetworkCore.ITransmittedObject
    {
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

        public ChatMessageModel(string message)
        {
            Message = message;
        }
    }
}
