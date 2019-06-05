using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server_V1
{
    class Server
    {
        const int port = 914;
        static TcpListener listener;
        static public int connections = 0;
        static void Main(string[] args)
        {
            
            try
            {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
                listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {  
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);
                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process)); 
                    clientThread.Start();
                    connections += 1;
                    Console.WriteLine("client connected. active connects: " + connections);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }

            /* socket.Bind(new IPEndPoint(IPAddress.Any,904));
             socket.Listen(5);
             Socket client = socket.Accept();
             Console.WriteLine("accept new");
             client.Receive(buffer);
             string login = reader.ReadString();
             string password = reader.ReadString();
             Console.WriteLine(login);
             Console.WriteLine(password);
             getUsersList();
             Console.ReadLine();*/
        }
        static public async void getUsersList()
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\visual studio\Projects\ConsoleApplication6\server_V1\Database.mdf;Integrated Security=True";
            SqlConnection sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            SqlDataReader sqlReader = null; 
            SqlCommand command = new SqlCommand("SELECT * FROM [User]", sqlConnecton);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    Console.WriteLine(sqlReader["name"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString(), ex.Source.ToString());
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }
    }
}
