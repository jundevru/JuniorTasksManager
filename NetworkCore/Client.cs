using System;
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
        private Action<bool>                _authResultDelegate;
        private bool                        connected           =   false;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip">адрес удаленного сервера</param>
        /// <param name="port">порт удаленного сервера</param>
        /// <param name="receiveDataDelegate">делегат, возвращающий полученный обьект ITransmittedObject</param>
        /// <param name="errorsInfoDelegate">делегат, возвращающий текст ошибки</param>
        public Client(string ip, int port, 
            Action<bool> authResultDelegate,
            Action<ITransmittedObject> receiveObjectDelegate, 
            Action<string> errorsInfoDelegate = null)
        {
            _serverIP = ip;
            _serverPort = port;
            if (_serverPort == 0)
                _serverPort = Utilits.DefaultPort;
            _errorsDelegate = errorsInfoDelegate;
            _receiveDelegate = receiveObjectDelegate;
            _authResultDelegate = authResultDelegate;
        }


        /// <summary>
        /// Подключиться к серверу
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Отключиться от сервера
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Передача объекта по сети, всем клиентам
        /// </summary>
        /// <param name="obj">Объект ITransmittedObject</param>
        /// <returns></returns>
        public bool SendObjectToAll(ITransmittedObject obj)
        {
            if (!connected)
            {
                _errorsDelegate?.Invoke("Подключение не открыто");
                return false;
            }
            byte[] data = Utilits.SerializeToBytes<ITransmittedObject>(obj);
            SendHeaderAndData(data);
            return true;
        }
        /// <summary>
        /// Передача объекта по сети конкретному пользователю
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool SendObjectToClient(ITransmittedObject obj, string userName)
        {
            if (!connected)
            {
                _errorsDelegate?.Invoke("Подключение не открыто");
                return false;
            }
            SendHeaderAndData(Utilits.SerializeToBytes(new ObjectToUserTransmitted(userName)));
            SendHeaderAndData(Utilits.SerializeToBytes(obj));
            return true;
        }
        /// <summary>
        /// Попытка авторизации на сервере с именем
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool SendAuth(string name)
        {
            if (!connected)
            {
                _errorsDelegate?.Invoke("Подключение не открыто");
                return false;
            }
            byte[] data = Utilits.SerializeToBytes(new NetworkAuthTransmitted(name));
            SendHeaderAndData(data);
            return true;
        }


        private void Listen()
        {
            while(_clientSocket.Connected)
            {
                ReceiveCommand(ReceiveHeaderAndData());
            }
            Disconnect();
        }
        private void ReceiveCommand(byte[] recevedData)
        {
            ITransmittedObject command = Utilits.DeserializeFromByte<ITransmittedObject>(recevedData);
            if (command is NetworkAuthTransmitted)
            {
                string result = (command as NetworkAuthTransmitted).name;
                _authResultDelegate.Invoke(result != "error");
                return;
            }
            _receiveDelegate?.Invoke(command);
        }


        private bool SendHeaderAndData(byte[] data)
        {
            byte[] header = Utilits.GetHeader(data.Length);
            _clientSocket.Send(header);
            _clientSocket.Send(data);
            return true;
        }
        private byte[] ReceiveHeaderAndData()
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
