using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Unity_Server.Logging;
using Unity_Server.tcpClient;

namespace Unity_Server
{
    class Program
    {
        public static TcpListener Listener;
        public static List<Client> Clients = new List<Client>();
        public static Thread runThread;
        static void Main(string[] args)
        {
            Logger.log1("Server is starting..");
            Listener = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }) , 8966);

            Listener.Start();
            
            ConnectionHandling();
            Logger.log1("dssd");

            Console.ReadLine();
        }

        static void ConnectionHandling()
        {
            Logger.log2(Logger.Type.Important, "Conectionshandling started..");
            Thread.Sleep(2);
            while (true)
            {
                Logger.log2(Logger.Type.Important, "Waiting for Connection..");
                var tcpclient = Listener.AcceptTcpClient();
                var client = new Client(tcpclient);
                client.CLIENT_MESSAGE += (o, e) =>
                {
                    Logger.log2(Logger.Type.Success, e.Msg);
                    Clients.ForEach(async k => await k.sendMessage(e.Msg));
                };


                Clients.Add(client);

                Logger.log1($"Client connectet - {client.IP} ");
                
            }
        }

        
    } 
}
