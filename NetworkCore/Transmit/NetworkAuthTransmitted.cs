// Используется для организации авторизации
// Серверу отправляется имя.
// Получает в ответ имя если, успешно
// или error если не успешно
using System;

namespace NetworkCore
{
    [Serializable]
    class NetworkAuthTransmitted : ITransmittedObject
    {
        public string name;
        
        public NetworkAuthTransmitted(string name)
        {
            this.name = name;
        }
    }
}
