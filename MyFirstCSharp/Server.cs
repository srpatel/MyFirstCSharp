using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFirstCSharp
{
    public class Server
    {
        private readonly IPEndPoint localEndPoint;
        private readonly Socket server;
        private bool listening;
        private Thread accepter;

        public Server(int port)
        {
            localEndPoint = new IPEndPoint(IPAddress.Any, port);

            // Create a TCP/IP socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Stop()
        {
            listening = false;
            accepter.Interrupt();
        }

        private void AcceptConnections()
        {
            try
            {
                // Start a synchronous socket to listen for connections
                while (listening)
                {
                    Socket clientSocket = server.Accept();
                    if (clientSocket == null)
                    {
                        Console.WriteLine("Socket timed out");
                    } else {
                        // Start client to respond to messages
                        Console.WriteLine("Socket accepted");

                        // Choose which type of server to start.
                        //Client client = new Client(clientSocket, ClientResponders.EchoResponder);
                        //Client client = new Client(clientSocket, ClientResponders.FizzBuzzResponder);
                        Client client = new Client(clientSocket, ClientResponders.SimpleWebServer);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Start()
        {
            listening = true;
            try
            {
                // Bind the socket to the local endpoint and listen
                server.Blocking = true;
                server.Bind(localEndPoint);
                server.ReceiveTimeout = 1000;
                server.Listen(100);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Start a thread to listen for new connections
            accepter = new Thread(AcceptConnections);
            accepter.IsBackground = true;
            accepter.Start();
        }
    }
}
