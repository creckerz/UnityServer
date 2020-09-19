using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerClient
{
    class Program
    {
        static  async Task Main(string[] args)
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 8966);

            Console.WriteLine("Client connected!");

           var rt =  new Task(() => getMessages(client));
            rt.Start();

            while (true)
            {
               var k =  Console.ReadLine();
               await client.GetStream().WriteAsync(ASCIIEncoding.UTF8.GetBytes(k));

            }
        }

        static async Task getMessages(TcpClient client)
        {
            while(true)
            {
               
                if(client.GetStream().DataAvailable)
                {
                    // really dirty -- so all bytes will be theyre --> never use in real app!
                    await Task.Delay(500);
                    List<byte> data = new List<byte>();
                    while(client.GetStream().DataAvailable)
                    {
                        data.Add((byte)client.GetStream().ReadByte());
                    }

                    Console.WriteLine($"Rec data {ASCIIEncoding.UTF8.GetString(data.ToArray())}");


                }
                await Task.Delay(100);
            }
        }
    }
}
