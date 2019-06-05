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
        public UserProfile()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Methods gul = new Methods(Command.GetUsersList);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(MainWindow.stream, gul);
            gul = (Methods)formatter.Deserialize(MainWindow.stream);
            list = gul.GetUsersList();
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
            btn.Content = "Delete";
            MessageBox.Show(btn.Uid);
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox btn = (CheckBox)sender;
            btn.Content = "Friend";
            MessageBox.Show(btn.Uid);
        } 
    }
}
