using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetworkCore
{
    public static class Utilits
    {
        public static int DefaultPort = 6070;
        public static readonly int HeaderSize = 2048;
        public static string info = "";

        public static byte[] SerializeToBytes<T>(T obj) where T : class
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            try
            {               
                formatter.Serialize(ms, obj);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                info = ex.Message;
                return null;
            }
            finally
            {
                ms.Close();
            }
        }
        public static T DeserializeFromByte<T>(byte[] data) where T : class
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {               
                ms.Write(data, 0, data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
