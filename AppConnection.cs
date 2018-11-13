using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MusikApp
{
    class AppConnection
    {
        IPAddress ip;

        TcpListener server;
        TcpClient client;

        string serverIP = "192.168.178.28";
        int port = 8080;

        //public Thread readThread;

        public string command = "";

        public AppConnection()
        {
            //readThread = new Thread(Read);
            ip = IPAddress.Parse("192.168.178.22");

            server = new TcpListener(ip, 8080);
            client = default(TcpClient);

            try
            {
                server.Start();
                Console.WriteLine("Server started...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //readThread.Start();

            Task.Factory.StartNew(() => Read());
        }

        public void Read()
        {
            Console.WriteLine("network thread: " + Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("listening...");
            while (true)
            {
                client = server.AcceptTcpClient();

                byte[] receivedBuffer = new byte[100];
                NetworkStream stream = client.GetStream();
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                StringBuilder msg = new StringBuilder();
                foreach (byte b in receivedBuffer)
                {
                    if (b.Equals(59))
                    {
                        break;
                    }
                    else
                    {
                        msg.Append(Convert.ToChar(b).ToString());
                    }
                }
                this.command = msg.ToString();
            }
        }

        public void SendCommand(string cmd)
        {
            try
            {
                TcpClient client = new TcpClient(serverIP, port);

                int byteCount = Encoding.ASCII.GetByteCount(cmd + 1);

                byte[] sendData = new byte[byteCount];

                sendData = Encoding.ASCII.GetBytes(cmd + ";");

                NetworkStream stream = client.GetStream();

                stream.Write(sendData, 0, sendData.Length);

                stream.Close();
                client.Close();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
