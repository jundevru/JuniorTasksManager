using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagerClient.Statics
{
    interface IUpdateSenderReceiver
    {
        void SendUpdate();
        event Action ReceiveUpdate;
    }
}
