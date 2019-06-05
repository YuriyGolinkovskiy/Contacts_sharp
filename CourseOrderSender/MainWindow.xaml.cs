using System;
using System.Windows;

namespace CourseOrderSender
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = new CourseOrder
                {
                    Course = new Course()
                    {
                        Title = comboBoxCourses.Text
                    },
                    Customer = new Customer
                    {
                        Company = textCompany.Text,
                        Contact = textContact.Text
                    }
                };

                // Для частной очереди использовать
                // using (MessageQueue queue = new MessageQueue(@".\Private$\CourseOrder"))
                using (MessageQueue queue = new MessageQueue(
                    CourseOrder.CourseOrderQueueName))
                using (var message = new Message(order))
                {
                    if (checkBoxPriority.IsChecked == true)
                    {
                        message.Priority = MessagePriority.High;
                    }

                    message.Recoverable = true;
                    queue.Send(message, String.Format("Заказ курса {{{0}}}",
                        order.Customer.Company));
                }

                MessageBox.Show("Заказ курса отправлен", "Заказ курса",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (MessageQueueException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при создании заказа",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
