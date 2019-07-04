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
        private bool                _tryStop    =       true;

        /// <summary>
        /// Событие авторизации клиента с именем
        /// </summary>
        public event Action<string> ClientLoggedInEvent;
        /// <summary>
        /// Событие отключения клиента с именем
        /// </summary>
        public event Action<string> ClientLoggedOutEvent;

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
            if (!_tryStop)
                return;
            _tryStop = false;
            Client client = new Client("127.0.0.1", _mainPort, (res)=> { }, (o) => { });
            client.Connect();
            foreach (ServerClient c in clients)
                c.Disconnect();
        }

        private void Listen()
        {
            IPAddress adress = IPAddress.Parse(_mainHost);
            using (_mainSocket = new Socket(adress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                _mainSocket.Bind(new IPEndPoint(adress, _mainPort));
                _mainSocket.Listen(100);
                _isWork = true;
                while (_tryStop)
                {
                    Socket clientSocket = _mainSocket.Accept();
                    clients.Add(new ServerClient(clientSocket, 
                    (disconnectedClient)        => 
                    {
                        if (clients.Contains(disconnectedClient) && !_tryStop)
                        {
                            clients.Remove(disconnectedClient);
                            if (disconnectedClient.UserName != "")
                                ClientLoggedOutEvent?.Invoke(disconnectedClient.UserName);
                        }
                    },
                    (sendedToAllData)           => 
                    {
                        foreach (ServerClient client in clients)
                            client.SendHeaderAndData(sendedToAllData);
                    },
                    (sendToUserData, userName)  => 
                    {
                        clients.FirstOrDefault(c => c.UserName == userName)?.SendHeaderAndData(sendToUserData);
                    },
                    (userName)                  => {
                        if (clients.FirstOrDefault(c => c.UserName == userName) == null)
                        {
                            ClientLoggedInEvent?.Invoke(userName);
                            return true;
                        }
                        return false;
                    }));
                }
                _mainSocket.Shutdown(SocketShutdown.Both);
                _mainSocket.Close();
                _isWork = false;
            }
        }

    }
}
