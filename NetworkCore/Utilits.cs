using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetworkCore
{
    public static class Utilits
    {
        public static int DefaultPort = 6070;
        public static readonly int HeaderSize = 16;

        /// <summary>
        /// Заголовок фиксированной длинны, первый отправляемый в сеть, содержащий число - размер следующего массива данных
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GetHeader(int length)                                                      
        {
            string header = length.ToString();
            if (header.Length < HeaderSize)
            {
                string zeros = null;
                for (int i = 0; i < (HeaderSize - header.Length); i++)
                {
                    zeros += "0";
                }
                header = zeros + header;
            }
            byte[] byteheader = Encoding.Unicode.GetBytes(header);
            return byteheader;
        }

        public static byte[] SerializeToBytes<T>(T obj) where T : class
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                formatter.Serialize(ms, obj);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static T DeserializeFromByte<T>(byte[] data) where T : class
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
