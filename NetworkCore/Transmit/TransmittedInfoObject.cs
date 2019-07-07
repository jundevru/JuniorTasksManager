using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore
{
    enum TransmittedDataType
    {
        TransmittedObject,
        Bytes
    }
    [Serializable]
    class TransmittedInfoObject : ITransmittedObject
    {
        public long length;
        public string to;
        public TransmittedDataType type;

        public TransmittedInfoObject(string to, long length, TransmittedDataType type)
        {
            this.to = to;
            this.length = length;
            this.type = type;
        }
    }
}
