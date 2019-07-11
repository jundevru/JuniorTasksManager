using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace NetworkCore
{
    public class Server
    {
        public bool                 IsWork      =>      _isWork;
        public int                  Connections =>      clients.Count;
        private List<ServerClient>  clients     =       new List<ServerClient>();
        private Thread              _mainThread;
        private Socket              _mainSocket;
        private string              _mainHost   =       "127.0.0.1";
        private int                 _mainPort;
        private bool                _isWork     =       false;
        private bool                _tryStop    =       false;

        /// <summary>
        /// Событие авторизации клиента с именем
        /// </summary>
        public event Action<string> ClientLoginEvent;
        /// <summary>
        /// Событие отключения клиента с именем
        /// </summary>
        public event Action<string> ClientLogoutEvent;      
        /// <summary>
        /// Сервер остановлен
        /// </summary>
        public event Action ServerStopped;
        /// <summary>
        /// Сервер запущен
        /// </summary>
        public event Action ServerStarted;
        /// <summary>
        /// Изменено количество подключений
        /// </summary>
        public event Action<int> ConnectionsChange;


        public Server(int port = 0)
        {
            _mainPort = port;
            if (_mainPort == 0)
                _mainPort = Utilits.DefaultPort;
            _mainThread = new Thread(Listen);
            _mainThread.IsBackground = true;
            _mainThread.Start();
        }

        /// <summary>
        /// Команда на остановку сервера
        /// </summary>
        public void TryStop()
        {
            if (_tryStop)
                return;
            _tryStop = true;          
            Client client = new Client("127.0.0.1", _mainPort, (res)=> { }, (res) => { }, (o) => { }, ()=> { });
            client.Connect();
        }

        private void Listen()
        {
            IPAddress adress = IPAddress.Parse(_mainHost);
            using (_mainSocket = new Socket(adress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                _mainSocket.Bind(new IPEndPoint(adress, _mainPort));
                _mainSocket.Listen(100);
                _isWork = true;
                ServerStarted?.Invoke();
                ConnectionsChange?.Invoke(clients.Count);
                while (!_tryStop)
                {
                    
                    Socket clientSocket = _mainSocket.Accept();
                    lock (clients)
                    {
                        clients.Add(new ServerClient(clientSocket,
                        (disconnectedClient) =>
                        {
                            lock (clients)
                            {
                                if (clients.Contains(disconnectedClient) && !_tryStop)
                                {
                                    clients.Remove(disconnectedClient);
                                    if (disconnectedClient.UserName != "")
                                        ClientLogoutEvent?.Invoke(disconnectedClient.UserName);
                                }
                                ConnectionsChange?.Invoke(clients.Count);
                            }
                        },
                        (header, sendedToAllData) =>
                        {
                            lock (clients)
                            { 
                            foreach (ServerClient client in clients)
                                client.SendData(header, sendedToAllData);
                            }
                        },
                        (header, sendToUserData, userName) =>
                        {
                            lock (clients)
                            {
                                clients.FirstOrDefault(c => c.UserName == userName)?.SendData(header, sendToUserData);
                            }
                        },
                        (userName) =>
                        {
                            lock (clients)
                            {
                                if (clients.FirstOrDefault(c => c.UserName == userName) == null)
                                {
                                    ClientLoginEvent?.Invoke(userName);
                                    return true;
                                }
                            }
                            return false;
                        }));
                    
                        ConnectionsChange?.Invoke(clients.Count);
                    }
                }
                lock (clients)
                {
                    foreach (ServerClient c in clients)
                        c.Disconnect();
                    clients.Clear();
                }
                //_mainSocket.Shutdown(SocketShutdown.Both);
                _mainSocket.Close();
                _isWork = false;
                ServerStopped?.Invoke();
            }
        }

    }
}
