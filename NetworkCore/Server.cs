using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Server(int port = 0)
        {
            _mainPort = port;
            if (_mainPort == 0)
                _mainPort = Utilits.DefaultPort;
            _mainThread = new Thread(Listen);
            _mainThread.IsBackground = true;
            _mainThread.Start();
        }

        public void TryStop()
        {
            _tryStop = false;
            Client client = new Client("127.0.0.1", _mainPort, (data) => { });
            client.Connect();
            client.Disconnect();
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
                    clients.Add(new ServerClient(clientSocket, (s)=>
                    {
                        if (clients.Contains(s))
                            clients.Remove(s);
                    },
                    (d)=> 
                    {
                        foreach (ServerClient client in clients)
                            client.SendData(d);
                    }));
                }
                _mainSocket.Shutdown(SocketShutdown.Both);
                _mainSocket.Close();
                _isWork = false;
            }
        }

    }
}
