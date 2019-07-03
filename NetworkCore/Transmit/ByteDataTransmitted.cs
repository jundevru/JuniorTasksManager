using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore
{
    [Serializable]
    class ByteDataTransmitted : ITransmittedObject
    {
        public string to;
        public long length;
        public ByteDataTransmitted(long length, string to = "")
        {
            this.length = length;
            this.to = to;
        }
    }
}
