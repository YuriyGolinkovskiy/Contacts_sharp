using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Serialization;
using System.Xml.Linq;

namespace client_V3
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    /// 
   
    public partial class Login : Window
    {
        public static List<object> userData = null;
        public Login()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
  
            if (loginTextBox.Text != string.Empty)
            {
                if (passwordTextBox.Text != string.Empty)
                {
                    Methods au = new Methods(Command.Authorization);
                    au.Auth(loginTextBox.Text, passwordTextBox.Text);
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(MainWindow.stream, au);
                    au = (Methods)formatter.Deserialize(MainWindow.stream);
                    if (au.status)
                    {
                        userData = au.UserData();
                        Close();
                        UserProfile mainPage = new UserProfile();
                        mainPage.Show();
                    }
                    else
                    {
                        MessageBox.Show("Данные не верны");
                    }
                }
                else
                {
                    MessageBox.Show("Вы не ввели пароль");
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели логин");
            }
        }
    }
}
