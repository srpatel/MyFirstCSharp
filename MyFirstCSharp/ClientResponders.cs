using System;
namespace MyFirstCSharp
{
    public class ClientResponders
    {
        private ClientResponders()
        {
        }

        public static void EchoResponder(string message, Client client)
        {
            client.Write(message);
        }

        /**
         * Super simple webserver implementation.
         * No headers are read.
         * We do not send a Length header.
         * We don't respond correctly to e.g. OPTIONS requests, and just assume everything is GET.        
         */
        public static void SimpleWebServer(string message, Client client)
        {
            string[] lines = message.Split("\r\n"); // As per RFC2616

            // Fist line is method and URL separated by space
            string[] requestLineParts = lines[0].Split(" ", 2);
            if (requestLineParts.Length != 2)
            {
                Console.Error.Write("Unrecognised request.");
                return;
            }

            string method = requestLineParts[0];
            string url = requestLineParts[1];

            client.Write("HTTP/1.1 200 OK\r\n");
            client.Write("\r\n");
            client.Write($@"<html>
    <head><title>Simple WebServer</title></head>
    <body>
        <h1>Simple WebServer</h1>
        <p>URL requested: {url}</p>
    </body>
</html>");
            client.Write("\r\n");
            client.Close();
        }

        public static void FizzBuzzResponder(string message, Client client)
        {
            try
            {
                int i = Convert.ToInt32(message);
                client.Write(FizzBuzz.Calculate(i) + "\n");
            } catch (Exception e)
            {
                client.Write("Invalid number supplied:\n");
                client.Write(e.Message);
                client.Write("\nClosing socket.\n");
                client.Close();
            }
        }
    }
}
