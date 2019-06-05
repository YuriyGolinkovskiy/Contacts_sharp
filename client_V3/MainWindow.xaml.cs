using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace client_V3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Login loginForm = new Login();
        static public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static public NetworkStream stream;
        const int port = 914;
        const string address = "127.0.0.1";
        TcpClient client = null;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //socket.Connect("127.0.0.1", 904);
           
            try
            {
                client = new TcpClient(address, port);
               
                stream = client.GetStream();
                //// получаем ответ
                //byte[] data = new byte[64]; // буфер для получаемых данных
                //StringBuilder builder = new StringBuilder();
                //int bytes = 0;
                //do
                //{
                //    bytes = stream.Read(data, 0, data.Length);
                //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                //}
                //while (stream.DataAvailable);

                //string message = builder.ToString();
                //MessageBox.Show(message);
                //Console.WriteLine("Сервер: {0}", message);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //finally
            //{
            //    client.Close();
            //}
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            client.Close();
        }
    }
}
