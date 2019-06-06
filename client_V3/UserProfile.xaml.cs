using Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace client_V3
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class UserProfile : Window
    {
        List<List<object>> list = null;
        List<List<object>> friendsList = null;
        Methods addToFriend = new Methods(Command.AddToFriend);
        Methods delFromFriend = new Methods(Command.DelFromFriend);
        Methods getFriendList = new Methods(Command.GetFriendList);
        IFormatter formatter = new BinaryFormatter();
        public UserProfile()
        {
            InitializeComponent();
        }
        public void GetFriends()
        {
            getFriendList.idUser = Convert.ToInt32(Login.userData[0]);
            formatter.Serialize(MainWindow.stream, getFriendList);
            getFriendList = (Methods)formatter.Deserialize(MainWindow.stream);
            friendsList = getFriendList.GetFriends();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Methods gul = new Methods(Command.GetUsersList);
            formatter.Serialize(MainWindow.stream, gul);
            gul = (Methods)formatter.Deserialize(MainWindow.stream);
            list = gul.GetUsersList();

            GetFriends();
            listBox1.ItemsSource = friendsList;
            
            var n = 0;
            foreach (var item in list)
            {
                if (item[0].ToString() == Login.userData[0].ToString())
                {
                    list.RemoveAt(n);
                    break;
                }
                n++;
            }
            listBox.ItemsSource = list;
            
            name.Content = Login.userData[3];
            surname.Content = Login.userData[4];
            phone.Content = Login.userData[5];
            email.Content = Login.userData[2];
            login.Content = Login.userData[1];
            //listView.ItemsSource = list;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
  
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            addToFriend.AddToFriend(Convert.ToInt32(Login.userData[0]),Convert.ToInt32(btn.Uid));
            formatter.Serialize(MainWindow.stream, addToFriend);
            addToFriend = (Methods)formatter.Deserialize(MainWindow.stream);
            MessageBox.Show("Добавлен в контакты");
            GetFriends();
            listBox1.ItemsSource = friendsList;

        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            delFromFriend.AddToFriend(Convert.ToInt32(Login.userData[0]), Convert.ToInt32(btn.Uid));
            formatter.Serialize(MainWindow.stream, delFromFriend);
            delFromFriend = (Methods)formatter.Deserialize(MainWindow.stream);
            MessageBox.Show("Удален из контактов");
            GetFriends();
            listBox1.ItemsSource = friendsList;
        } 
    }
}
