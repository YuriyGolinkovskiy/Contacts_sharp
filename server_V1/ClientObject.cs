using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Serialization;
using System.Security.Cryptography;

namespace server_V1
{
    class ClientObject
    {
        public TcpClient client;
        //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+Environment.CurrentDirectory+@"Database.mdf;Integrated Security=True";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\visual studio\Projects\ConsoleApplication6\server_V1\Database.mdf;Integrated Security=True";
        NetworkStream stream = null;
        SqlDataReader sqlReader;
        SqlConnection sqlConnecton;
        SqlCommand command;
        string sSourceData;
        byte[] tmpSource;
        byte[] tmpHash;
        IFormatter formatter = new BinaryFormatter();
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }
/// <summary>
/// Авторизация
/// </summary>
        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
        public async void Authorization(Methods metod)
        {
            metod.userData = new List<object>();
            sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            command = new SqlCommand("SELECT * FROM [User]", sqlConnecton);
            try
            {
                sSourceData = metod.password;
                tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);
                tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    if ((metod.login == sqlReader["login"].ToString() || metod.login == sqlReader["email"].ToString()) && sqlReader["password"].ToString() == ByteArrayToString(tmpHash))
                    {
                        metod.userData.Add(sqlReader["id"]);
                        metod.userData.Add(sqlReader["login"]);
                        metod.userData.Add(sqlReader["email"]);
                        metod.userData.Add(sqlReader["name"]);
                        metod.userData.Add(sqlReader["surname"]);
                        metod.userData.Add(sqlReader["phone"]);
                        metod.status = true;
                        break;
                    }

                }
                formatter.Serialize(stream, metod);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
                if (sqlConnecton != null && sqlConnecton.State != System.Data.ConnectionState.Closed)
                {
                    sqlConnecton.Close();
                }
            }
        }
/// <summary>
/// Авторизация
/// </summary>

/// <summary>
/// Регистрация
/// </summary>
        public async void Registration(Methods metod)
        {
            metod.status = true;
            sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            command = new SqlCommand("SELECT * FROM [User]", sqlConnecton);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    if (metod.login == sqlReader["login"].ToString())
                    {
                        metod.login = null;
                        metod.status = false;
                        break;
                    }
                    if (metod.email == sqlReader["email"].ToString())
                    {
                        metod.email = null;
                        metod.status = false;
                        break;
                    }
                }
                if (metod.status)
                {
                    sSourceData = metod.password;
                    tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);
                    tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
                    sqlReader.Close();
                    SqlCommand command = new SqlCommand("INSERT INTO [User] (login, email, name, surname, phone, dateOfBirth, password)VALUES(@login, @email, @name, @surname, @phone, @dateOfBirth, @password)", sqlConnecton);
                    command.Parameters.AddWithValue("login", metod.login);
                    command.Parameters.AddWithValue("email", metod.email);
                    command.Parameters.AddWithValue("name", metod.name);
                    command.Parameters.AddWithValue("surname", metod.surname);
                    command.Parameters.AddWithValue("phone", metod.phone);
                    command.Parameters.AddWithValue("dateOfBirth", metod.dateOfBirth);
                    command.Parameters.AddWithValue("password", ByteArrayToString(tmpHash));
                    await command.ExecuteNonQueryAsync();
                }
                formatter.Serialize(stream, metod);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
               if (sqlReader != null)
                   sqlReader.Close();
               if (sqlConnecton != null && sqlConnecton.State != System.Data.ConnectionState.Closed)
               {
                   sqlConnecton.Close();
               }
            }
        }
/// <summary>
/// Регистрация
/// </summary>-
        public async void GetUsersList(Methods metod)
        {
            metod.list = new List<List<object>>();
            sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            command = new SqlCommand("SELECT * FROM [User]", sqlConnecton);

            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    metod.list.Add(new List<object>() { sqlReader["id"].ToString(), sqlReader["login"].ToString(), sqlReader["email"].ToString(), sqlReader["name"].ToString(), sqlReader["surname"].ToString(), sqlReader["phone"].ToString(), sqlReader["dateOfBirth"].ToString() });
                }
                formatter.Serialize(stream, metod);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
               if (sqlReader != null)
                   sqlReader.Close();
               if (sqlConnecton != null && sqlConnecton.State != System.Data.ConnectionState.Closed)
               {
                   sqlConnecton.Close();
               }
            }
        }
        public async void AddToFriend(Methods metod)
        {
            sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            command = new SqlCommand("INSERT INTO [Friends] (id_friend, id_user)VALUES(@id_friend, @id_user)", sqlConnecton);
            try
            {
               
                command.Parameters.AddWithValue("id_friend", metod.idFriend);
                command.Parameters.AddWithValue("id_user", metod.idUser);
                
                await command.ExecuteNonQueryAsync();
                formatter.Serialize(stream, metod);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
               if (sqlReader != null)
                   sqlReader.Close();
               if (sqlConnecton != null && sqlConnecton.State != System.Data.ConnectionState.Closed)
               {
                   sqlConnecton.Close();
               }
            }
        }
        public async void DelFromFriend(Methods metod)
        {
            sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            command = new SqlCommand($"DELETE FROM [Friends] WHERE id_friend = {metod.idFriend} and id_user = {metod.idUser}", sqlConnecton);
            try
            {
                await command.ExecuteNonQueryAsync();
                formatter.Serialize(stream, metod);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
                if (sqlConnecton != null && sqlConnecton.State != System.Data.ConnectionState.Closed)
                {
                    sqlConnecton.Close();
                }
            }
        }
        public async void GetFriendList(Methods metod)
        {
            metod.friendsList = new List<List<object>>();
            sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            //command = new SqlCommand($"SELECT * FROM [User] where id = (Select id_user FROM [Friends] where id_user = {metod.idUser})", sqlConnecton);
            command = new SqlCommand($"SELECT * FROM [User] where id = ANY (Select id_friend FROM [Friends] where id_user = {metod.idUser})", sqlConnecton);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                    metod.friendsList.Add( new List<object>() { sqlReader["id"].ToString(), sqlReader["login"].ToString(), sqlReader["email"].ToString(), sqlReader["name"].ToString(), sqlReader["surname"].ToString(), sqlReader["phone"].ToString(), sqlReader["dateOfBirth"].ToString(), sqlReader["password"].ToString() });
                }
                formatter.Serialize(stream, metod);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
                if (sqlConnecton != null && sqlConnecton.State != System.Data.ConnectionState.Closed)
                {
                    sqlConnecton.Close();
                }
            }
        }

        public void Process()
        {
            try
            {
                stream = client.GetStream();
                while (true) {
                    Methods method = (Methods)formatter.Deserialize(stream);
                    if (method.com == Command.Authorization)
                        Authorization(method);
                    if (method.com == Command.Registration)
                        Registration(method);
                    if (method.com == Command.GetUsersList)
                        GetUsersList(method);
                    if (method.com == Command.AddToFriend)
                        AddToFriend(method);
                    if (method.com == Command.DelFromFriend)
                        DelFromFriend(method);
                    if (method.com == Command.GetFriendList)
                        GetFriendList(method);







                    //string text;
                    //StringBuilder builder = new StringBuilder();
                    //int bytes = 0;
                    //do
                    //{
                    //    bytes = stream.Read(data, 0, data.Length);
                    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //}
                    //while (stream.DataAvailable);

                    //text = builder.ToString();

                    //Console.WriteLine(text);
                    //string message = "Данные получены";
                    //   // int bytes = stream.Read(data, 0, data.Length); //(**This receives the data using the byte method**)
                    //   // message = System.Text.Encoding.Unicode.GetString(data, 0, bytes); //(**This converts it to string**)
                    //data = Encoding.Unicode.GetBytes($"{message}");
                    //    stream.Write(data, 0, data.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                {
                    client.Close();
                    Server.connections -= 1;
                    Console.WriteLine("client disconnected. active connects: " + Server.connections);
                }
                   
            }
        }
    }
}
