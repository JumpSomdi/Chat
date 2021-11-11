using System;
using System.Net.Sockets;
using System.Text;

namespace Chat
{
    class Client
    {
        private TcpClient tcpClient;
        private Server server;

        public string userName;
        public string Id;
        public NetworkStream networkStream;

        public Client(TcpClient tcp, Server _server)
        {
            //открываем подключение для нового клиента
            Id = Guid.NewGuid().ToString();
            tcpClient = tcp;
            server = _server;
        }
        public void Process()
        {
            try
            {
                networkStream = tcpClient.GetStream();

                userName = GetMessage();
                string firstMessage = String.Format("{0} join to the server", userName);
                server.BrodcustMessage(firstMessage, this);

                while (true)
                {
                    try
                    {
                        string newMessage = GetMessage();
                        newMessage = String.Format("{0}: {1}", userName, newMessage);
                        server.BrodcustMessage(newMessage, this);
                    }
                    catch
                    {
                        string lastMessage = String.Format("{0} left the chat", userName);
                        server.BrodcustMessage(lastMessage, this);
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.DeleteUser(Id);
                Close();
            }
        }

        private string GetMessage()
        {
            byte[] message = new byte[64];
            int countReadedByte = 0;
            StringBuilder builder = new StringBuilder();

            do
            {
                countReadedByte = networkStream.Read(message, 0, message.Length);
                builder.Append(Encoding.UTF8.GetString(message), 0, countReadedByte);
            }
            while (networkStream.DataAvailable);

            return builder.ToString();
        }

        public void Close()
        {
            if (networkStream != null) networkStream.Close();
            if (tcpClient != null) tcpClient.Close();
        }
    }
}
