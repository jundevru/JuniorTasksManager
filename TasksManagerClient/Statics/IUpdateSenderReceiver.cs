// Отправитель/получатель информации об обновлении данных в базе
using System;

namespace TasksManagerClient.Statics
{
    interface IUpdateSenderReceiver
    {
        void SendUpdate();
        event Action ReceiveUpdate;
    }
}
