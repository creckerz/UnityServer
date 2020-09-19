using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity_Server.Logging;

namespace Unity_Server.tcpClient
{
    class Client
    {
        public TcpClient _Client;
        Thread HandleClientThread;

        public delegate void ClientMessageHandler(object sender, ClientEventargs eventargs);

        public event ClientMessageHandler CLIENT_MESSAGE;
        protected virtual void onCLIENT_MESSAGE(ClientEventargs eventargs)
        {
            CLIENT_MESSAGE?.Invoke(this, eventargs);
        }
        volatile List<byte> buffer = new List<byte>();
        public IPAddress IP
        {
            get
            {
                return ((IPEndPoint)_Client.Client.RemoteEndPoint).Address;
            }
        }

        public Client(TcpClient client)
        {
            _Client = client;
            HandleClientThread = new Thread(new ThreadStart(HandleClient));
            HandleClientThread.Start();

        }

        public async Task sendMessage(string msg)
        {
            await sendMessage(ASCIIEncoding.UTF8.GetBytes(msg));
        }
        public async Task sendMessage(byte[] msg)
        {
           await _Client.GetStream().WriteAsync(msg);
        }
        private void HandleClient()
        {
            Logger.log2(Logger.Type.Important, $"HandleClient -  {IP.ToString()}");
            while(true)
            {
                if(_Client.GetStream().DataAvailable)
                {
                    while(_Client.GetStream().DataAvailable)
                    {
                        buffer.Add((byte)_Client.GetStream().ReadByte());
                    }
                    ParseBuffer();

                }
            }

        }

        private void ParseBuffer()
        {
            // Dummyimplementation

            if(buffer.Count> 10)
            {
                string msg = ASCIIEncoding.UTF8.GetString(buffer.GetRange(0, 10).ToArray());
                buffer.RemoveRange(0, 10);
                onCLIENT_MESSAGE(new ClientEventargs() { Msg = msg });

            }
        }
    }
}
