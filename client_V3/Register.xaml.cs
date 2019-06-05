using Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
using System.Xml.Linq;

namespace client_V3
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    { 
        public Register()
        {
            InitializeComponent();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Methods reg = new Methods(Command.Registration);
            if (loginTextBox.Text != string.Empty && emailTextBox.Text != string.Empty && nameTextBox.Text != string.Empty && surnameTextBox.Text != string.Empty && phoneTextBox.Text != string.Empty && passwordRepeatTextBox.Text != string.Empty && loginTextBox.Text != string.Empty)
            {
                if (passwordTextBox.Text == passwordRepeatTextBox.Text)
                {
                    reg.Registration(loginTextBox.Text, emailTextBox.Text, nameTextBox.Text, surnameTextBox.Text, phoneTextBox.Text, dateTextBox.Text, passwordTextBox.Text);
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(MainWindow.stream, reg);
                    reg = (Methods)formatter.Deserialize(MainWindow.stream);
                    if (reg.login == null)
                    {
                        MessageBox.Show("Логин занят");
                    }
                    else
                    {
                        if (reg.email == null)
                            MessageBox.Show("email занят");
                        else
                        {
                            MessageBox.Show("Успешная регистрация");
                            Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают");
                }
            }
            else
            {
                MessageBox.Show("Заполнены не все поля");
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
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
    }
}
