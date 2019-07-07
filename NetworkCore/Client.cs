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
        private Action<ITransmittedObject>  _receiveObjectDelegate;
        private Action<byte[]>              _receiveBytesDelegate;
        private Action<bool>                _authResultDelegate;
        private Action                      _disconnectDelegate;
        private bool                        connected           =   false;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ip">адрес удаленного сервера</param>
        /// <param name="port">порт удаленного сервера</param>
        /// <param name="authResultDelegate">делегат, возвращающий результат авторизации</param>
        /// <param name="receiveObjectDelegate">делегат, возвращающий полученный обьект ITransmittedObject</param>
        /// <param name="errorsInfoDelegate">делегат, возвращающий текст ошибки</param>
        public Client(string ip, int port, 
            Action<bool> authResultDelegate,
            Action<ITransmittedObject> receiveObjectDelegate, 
            Action<byte[]> receiveBytesDelegate,
            Action disconnectDelegate,
            Action<string> errorsInfoDelegate = null)
        {
            _serverIP = ip;
            _serverPort = port;
            if (_serverPort == 0)
                _serverPort = Utilits.DefaultPort;
            _errorsDelegate = errorsInfoDelegate;
            _receiveObjectDelegate = receiveObjectDelegate;
            _receiveBytesDelegate = receiveBytesDelegate;
            _authResultDelegate = authResultDelegate;
            _disconnectDelegate = disconnectDelegate;
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

        #region Send
        /// <summary>
        /// Send Object to all or client
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool SendObject(ITransmittedObject obj, string to = "")
        {
            if (!connected)
            {
                _errorsDelegate?.Invoke("Подключение не открыто");
                return false;
            }
            byte[] data = Utilits.SerializeToBytes<ITransmittedObject>(obj);
            TransmittedInfoObject header = new TransmittedInfoObject(to, data.Length, TransmittedDataType.TransmittedObject);
            _clientSocket.Send(Utilits.SerializeToBytes(header));
            _clientSocket.Send(data);
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
            _clientSocket.Send(Utilits.SerializeToBytes(new NetworkAuthTransmitted(name)));
            return true;
        }
        #endregion

        #region Receive
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
                }catch(Exception ex)
                {
                    _errorsDelegate?.Invoke(ex.Message);
                    continue;
                }
                ReceiveCommand(header);
            }
            Disconnect();
            _disconnectDelegate?.Invoke();
        }
        private void ReceiveCommand(byte[] recevedData)
        {
            ITransmittedObject command = Utilits.DeserializeFromByte<ITransmittedObject>(recevedData);
            if (command is NetworkAuthTransmitted)
            {
                string result = (command as NetworkAuthTransmitted).name;
                _authResultDelegate.Invoke(result != "error");
                if (result == "error")
                    _errorsDelegate?.Invoke("Пользователь с таким ником уже подключен");
                return;
            }
            if (command is TransmittedInfoObject)
            {
                var obj = command as TransmittedInfoObject;
                byte[] data = new byte[obj.length];
                int readed = _clientSocket.Receive(data);
                if (obj.type == TransmittedDataType.Bytes)
                    _receiveBytesDelegate?.Invoke(data);
                else
                    _receiveObjectDelegate?.Invoke(Utilits.DeserializeFromByte<ITransmittedObject>(data));
                return;
            }
            _errorsDelegate?.Invoke("Приняты неизвестные данные");
        }
        #endregion




    }
}
