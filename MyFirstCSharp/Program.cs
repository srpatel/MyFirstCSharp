using System;

namespace MyFirstCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8080);
            server.Start();

            Console.WriteLine("Server running. Press any key to stop.");
            Console.ReadKey();

            server.Stop();
        }
    }
}
