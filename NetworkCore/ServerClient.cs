using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkCore
{
    class ServerClient
    {
        private Thread                  _clientThread;
        private Socket                  _clientSocket;
        private Action<ServerClient>    _disconnectDelegate;
        private Action<byte[], byte[]>          _sendToAllDelegate;
        private Func<string, bool>      _setNameDelegate;
        private Action<byte[], byte[], string>  _sendToUserDelegate;
        private string                  userName;
        public  string                  UserName                =>      userName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket">Сокет клиента</param>
        /// <param name="disconnectDelegate">делегат, вызывающийся при закрытии соединения</param>
        /// <param name="sendToAllDelegate">делегат, пересылки пакета всем клиентам</param>
        public ServerClient(Socket socket, 
            Action<ServerClient> disconnectDelegate, 
            Action<byte[], byte[]> sendToAllDelegate, 
            Action<byte[], byte[], string> sendToUserDelegate, 
            Func<string,bool> setNameDelegate)
        {
            _clientSocket = socket;
            _disconnectDelegate = disconnectDelegate;
            _sendToAllDelegate = sendToAllDelegate;
            _sendToUserDelegate = sendToUserDelegate;
            _setNameDelegate = setNameDelegate;
            _clientThread = new Thread(Listen);
            _clientThread.IsBackground = true;
            _clientThread.Start();
        }

        public void Disconnect()
        {
            _disconnectDelegate.Invoke(this);
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Close();
        }

        /// <summary>
        /// Поток принимающий данные от удаленного клиента, пока есть соединение
        /// </summary>
        private void Listen()
        {
            while(_clientSocket.Connected)
            {
                ReceiveCommand(ReceiveHeaderAndData());
            }
            Disconnect();
        }
        /// <summary>
        /// Данные от клиента рассылаем всем
        /// </summary>
        /// <param name="recevedData"></param>
        private void ReceiveCommand(byte[] recevedData)
        {
            ITransmittedObject command = Utilits.DeserializeFromByte<ITransmittedObject>(recevedData);
            if (command is NetworkAuthTransmitted)
            {
                string name = (command as NetworkAuthTransmitted).name;
                byte[] data;
                if (_setNameDelegate.Invoke(name))
                {
                    userName = name;
                    data = recevedData;
                }
                else
                {
                    data = Utilits.SerializeToBytes(new NetworkAuthTransmitted("error"));
                }
                SendHeaderAndData(data, null);
                return;
            }
            if (command is ObjectToUserTransmitted)
            {
                string userName = (command as ObjectToUserTransmitted).name;
                byte[] data = ReceiveHeaderAndFirstData();
                _sendToUserDelegate?.Invoke(data, null, userName);
                return;
            }
            if (command is ByteDataTransmitted)
            {
                var obj = command as ByteDataTransmitted;
                byte[] data = new byte[obj.length];
                _clientSocket.Receive(data);
            }

            _sendToAllDelegate.Invoke(recevedData, null);
        }


        public bool SendHeaderAndData(byte[] firstData, byte[] secondData)
        {
            byte[] header = Utilits.GetHeader(firstData.Length);
            _clientSocket.Send(header);
            _clientSocket.Send(firstData);
            if (secondData != null)
                _clientSocket.Send(secondData);
            return true;
        }
        private byte[] ReceiveHeaderAndFirstData()
        {
            byte[] header = new byte[Utilits.HeaderSize];
            _clientSocket.Receive(header);
            int dataLength = int.Parse(Encoding.Unicode.GetString(header));
            byte[] data = new byte[dataLength];
            _clientSocket.Receive(data);
            return data;
        }
    }
}
