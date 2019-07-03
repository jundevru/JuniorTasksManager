// Используется для отправки объекта конкретному пользователю
using System;

namespace NetworkCore
{
    [Serializable]
    class ObjectToUserTransmitted : ITransmittedObject
    {
        public string name;

        public ObjectToUserTransmitted(string name)
        {
            this.name = name;
        }
    }
}
