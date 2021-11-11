using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Listen();
        }
    }
}
