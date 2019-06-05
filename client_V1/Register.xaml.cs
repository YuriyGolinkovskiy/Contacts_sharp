using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

namespace client_V1
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        SqlConnection sqlConnecton;
        public Register()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\visual studio\Projects\ConsoleApplication6\server_V1\Database.mdf;Integrated Security=True";
            sqlConnecton = new SqlConnection(connectionString);
            await sqlConnecton.OpenAsync();
            SqlDataReader sqlReader = null;
            SqlCommand UsersList = new SqlCommand("SELECT * FROM [User]", sqlConnecton);
            try
            {
                sqlReader = await UsersList.ExecuteReaderAsync();
                while (await sqlReader.ReadAsync())
                {
                   MessageBox.Show(sqlReader["name"].ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private async void OK_Click(object sender, RoutedEventArgs e)
        {
            if(loginTextBox.Text != string.Empty && emailTextBox.Text != string.Empty && nameTextBox.Text != string.Empty && surnameTextBox.Text != string.Empty && phoneTextBox.Text != string.Empty && passwordRepeatTextBox.Text != string.Empty && loginTextBox.Text != string.Empty)
            {
                if (passwordTextBox.Text == passwordRepeatTextBox.Text)
                {
                    SqlCommand command = new SqlCommand("INSERT INTO [User] (login, email, name, surname, phone, dateOfBirth, password)VALUES(@login, @email, @name, @surname, @phone, @dateOfBirth, @password)", sqlConnecton);
                    command.Parameters.AddWithValue("login", loginTextBox.Text);
                    command.Parameters.AddWithValue("email", emailTextBox.Text);
                    command.Parameters.AddWithValue("name", nameTextBox.Text);
                    command.Parameters.AddWithValue("surname", surnameTextBox.Text);
                    command.Parameters.AddWithValue("phone", phoneTextBox.Text);
                    command.Parameters.AddWithValue("dateOfBirth", dateTextBox.Text);
                    command.Parameters.AddWithValue("password", passwordTextBox.Text);
                    await command.ExecuteNonQueryAsync();
                    Close();
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

        private void RegisterForm_Closed(object sender, EventArgs e)
        {
            if (sqlConnecton != null && sqlConnecton.State != System.Data.ConnectionState.Closed)
            {
                sqlConnecton.Close();
            }
        }
    }
}
