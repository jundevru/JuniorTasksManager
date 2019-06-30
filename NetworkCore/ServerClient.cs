using System;
using System.Collections.Generic;
using System.Linq;
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
        private Action<byte[]>          _sendToAllDelegate;

        public ServerClient(Socket socket, Action<ServerClient> disconnectDelegate, Action<byte[]> sendToAllDelegate)
        {
            _clientSocket = socket;
            _disconnectDelegate = disconnectDelegate;
            _sendToAllDelegate = sendToAllDelegate;
            _clientThread = new Thread(Listen);
            _clientThread.IsBackground = true;
            _clientThread.Start();
        }

        /// <summary>
        /// Поток принимающий данные от удаленного клиента, пока есть соединение
        /// </summary>
        private void Listen()
        {
            while(_clientSocket.Connected)
            {
                byte[] header = new byte[Utilits.HeaderSize];
                _clientSocket.Receive(header);
                int dataLength = int.Parse(Encoding.Unicode.GetString(header));
                byte[] data = new byte[dataLength];
                _clientSocket.Receive(data);
                ReceiveCommand(data);
            }
            Disconnect();
        }

        /// <summary>
        /// Данные от клиента рассылаем всем
        /// </summary>
        /// <param name="recevedData"></param>
        private void ReceiveCommand(byte[] recevedData)
        {
            //ITransmittedObject command = Utilits.DeserializeFromByte<ITransmittedObject>(recevedData);
            _sendToAllDelegate.Invoke(recevedData);
        }

        private void Disconnect()
        {
            _disconnectDelegate.Invoke(this);
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Close();
        }

        public bool SendData(byte[] data)
        {
            byte[] header = Utilits.GetHeader(data.Length);
            _clientSocket.Send(header);
            _clientSocket.Send(data);
            return true;
        }
    }
}
