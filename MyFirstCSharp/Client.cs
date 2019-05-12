using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MyFirstCSharp
{
    public class Client
    {
        private readonly Socket socket;
        private readonly byte[] buffer;
        private readonly Thread reader;
        private readonly OnReceive responder;
        private bool reading;
        public Client(Socket socket, OnReceive responder)
        {
            this.socket = socket;
            this.buffer = new byte[1024];
            this.responder = responder;
            this.reading = true;

            // Start a thread to read data
            reader = new Thread(ReadData);
            reader.IsBackground = true;
            reader.Start();
        }

        public delegate void OnReceive(string message, Client client);

        private void ReadData()
        {
            try 
            {
                while (true)
                {
                    int read = socket.Receive(buffer);
                    if (read == 0)
                    {
                        // Socket closed by client.
                        break;
                    } 
                    else
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, read);
                        responder(message, this);
                    }
                }
            } 
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            } finally
            {
                if (reading)
                {
                    // Only close if we haven't already closed
                    // For example, we might close in the responder.
                    Close();
                }
            }

        }

        public void Write(string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }

        public void Close()
        {
            this.reading = false;
            Console.WriteLine("Socket closed");
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            reader.Interrupt();
        }
    }
}
