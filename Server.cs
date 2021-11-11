using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Chat
{
    class Server
    {
        private TcpListener listener = new TcpListener(IPAddress.Any, 7000);
        private List<Client> clients = new List<Client>();
        public void Listen()
        {
            listener.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                try
                {
                    //подключает новых клиентов и добавляет в список
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    Console.WriteLine("New client join");

                    Client client = new Client(tcpClient, this);
                    clients.Add(client);
                    Thread thread = new Thread(new ThreadStart(client.Process));
                    thread.Start();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Disconecte();
                }
            }
        }

     /* public void SendAnserw(Client cl, string ans)
        {
            cl.networkStream.Write(Encoding.UTF8.GetBytes(ans), 0, ans.Length);
        } */

        public void BrodcustMessage(string message, Client sender)
        {
            //созаём поток для записи нового сообщения для всех клиентов, которые подключены, кроме того клиента,
            //который отправил сообщение
            byte[] recivedMessage = Encoding.UTF8.GetBytes(message);

            for(int i = 0; i < clients.Count; i++)
            {
                //if (clients[i].Id != sender.Id) clients[i].networkStream.Write(recivedMessage, 0, recivedMessage.Length);
                clients[i].networkStream.Write(recivedMessage, 0, recivedMessage.Length);
            }

            Console.WriteLine("{0}: {1}", sender.userName, message);
        }

        public void Disconecte()
        {
            for(int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
        }

        public void DeleteUser(string Id)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id == Id)
                {
                    //i = clients.Count;
                    Client client = clients[i];
                    if (client != null)
                    {
                        Console.WriteLine(clients[i].userName + "left the chat");
                        clients.Remove(client);
                    }
                }
            }
        }
    }
}

