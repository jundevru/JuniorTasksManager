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
        private string                  userName = "";
        public  string                  UserName                =>      userName;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="socket">Сокет клиента</param>
        /// <param name="disconnectDelegate">делегат, вызывающийся при закрытии соединения</param>
        /// <param name="sendToAllDelegate">делегат, пересылки пакета всем клиентам</param>
        /// <param name="sendToUserDelegate">делегат, пересылки пакета клиенту по имени</param>
        /// <param name="setNameDelegate">делегат установки имени клиента</param>
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
            _disconnectDelegate?.Invoke(this);
            _clientSocket.Close();
        }


        private void Listen()
        {
            while(_clientSocket.Connected)
            {
                byte[] header = new byte[Utilits.HeaderSize];
                try
                {
                    int readed = _clientSocket.Receive(header);
                    if (readed == 0)
                        continue;
                }catch(Exception)
                {
                    _disconnectDelegate?.Invoke(this);
                    continue;
                }
                ReceiveCommand(header);
            }
            Disconnect();
        }

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
                _clientSocket.Send(data);
                return;
            }
            if (command is TransmittedInfoObject)
            {
                TransmittedInfoObject obj = command as TransmittedInfoObject;
                byte[] data = new byte[obj.length];
                int readed = _clientSocket.Receive(data);
                if (obj.to == "")
                    _sendToAllDelegate?.Invoke(recevedData, data);
                else
                    _sendToUserDelegate?.Invoke(recevedData, data, obj.to);
            }
        }

        public void SendData(byte[] header, byte[] data)
        {
            _clientSocket.Send(header);
            _clientSocket.Send(data);
        }

    }
}
