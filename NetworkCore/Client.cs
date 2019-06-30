using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace NetworkCore
{
    public class Client
    {
        public  bool                        IsConnected         =>  connected;
        private Socket                      _clientSocket;
        private Thread                      _listenThread;
        private string                      _serverIP;
        private int                         _serverPort;
        private Action<string>              _errorsDelegate;
        private Action<ITransmittedObject>  _receiveDelegate;
        private bool                        connected           =   false;

        public Client(string ip, int port, 
            Action<ITransmittedObject> receiveDataDelegate, 
            Action<string> errorsInfoDelegate = null)
        {
            _serverIP = ip;
            _serverPort = port;
            if (_serverPort == 0)
                _serverPort = Utilits.DefaultPort;
            _errorsDelegate = errorsInfoDelegate;
        }

        public bool Connect()
        {
            if (connected)
            {
                _errorsDelegate?.Invoke("Подключение уже открыто");
                return false;
            }
            try
            {
                var temp = IPAddress.Parse(_serverIP);
                _clientSocket = new Socket(temp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _clientSocket.Connect(new IPEndPoint(temp, _serverPort));
            }
            catch (Exception ex)
            {
                _errorsDelegate?.Invoke(ex.Message);
                return false;
            }
            connected = true;
            _listenThread = new Thread(Listen);
            _listenThread.IsBackground = true;
            _listenThread.Start();
            return true;
        }

        public bool Disconnect()
        {
            if (!connected)
            {
                _errorsDelegate?.Invoke("Подключение не открыто");
                return false;
            }
            connected = false;
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Close();
            return true;
        }

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
        /// Разбор полученной команды от сервера
        /// </summary>
        /// <param name="recevedData"></param>
        private void ReceiveCommand(byte[] recevedData)
        {
            ITransmittedObject command = Utilits.DeserializeFromByte<ITransmittedObject>(recevedData);
            _receiveDelegate?.Invoke(command);
        }

        private bool SendData(byte[] data)
        {
            byte[] header = Utilits.GetHeader(data.Length);
            _clientSocket.Send(header);
            _clientSocket.Send(data);
            return true;
        }

        public bool SendObject(ITransmittedObject obj)
        {
            if (!connected)
                return false;
            byte[] data = Utilits.SerializeToBytes<ITransmittedObject>(obj);
            SendData(data);
            return true;
        }
    }
}
