// Менеджер обработки сообщений об обновлении данных и рассылки их получателям
using System.Collections.Generic;

namespace TasksManagerClient.Statics
{
    class UpdatesMessagesManager
    {
        public static UpdatesMessagesManager Instance => instance; 
        static UpdatesMessagesManager instance;
        static UpdatesMessagesManager()
        {
            instance = new UpdatesMessagesManager();
        }

        private List<IUpdateSenderReceiver> list = new List<IUpdateSenderReceiver>();
        public void Add(IUpdateSenderReceiver obj)
        {
            list.Add(obj);
            obj.ReceiveUpdate += () => {
                foreach (var item in list)
                    if (item != null && !item.Equals(obj))
                        item.SendUpdate();
            };
        }
    }
}
