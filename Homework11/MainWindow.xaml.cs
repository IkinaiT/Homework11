using System;
using System.IO;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Homework11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _path = "UserData.txt";
        private List<User> _users = new List<User>();
        private List<User> _redactedUsers = new List<User>();
        private List<Employee> _employees = new List<Employee>();

        public MainWindow()
        {
            FillUsersList();

            

            InitializeComponent();
            ListBox1.ItemsSource = _redactedUsers;
            FillEmployees();
            
        }

        /// <summary>
        /// Изменение данных ListBox'а при изменении пользователя
        /// </summary>
        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LockAll();
            CheckUser();
            ConfirmButton.IsEnabled = true;
        }

        /// <summary>
        /// Заполнение списка клиентов из файла
        /// </summary>
        private void FillUsersList()
        {
            try
            {
                _users.Clear();
                using (StreamReader sr = new StreamReader(_path))
                {
                    while (!sr.EndOfStream)
                    {
                        string userData = sr.ReadLine();
                        string[] userLines = userData.Split('~');
                        _users.Add(new User(userLines[0], userLines[1], userLines[2], userLines[3], userLines[4], userLines[5], userLines[6]));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить список клиентов");
            }
            

        }

        /// <summary>
        /// Описание события при нажатии кнопки "Принять"
        /// </summary>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckOnConfirmClick())
            {
                try
                {
                    if(_employees[ComboBox1.SelectedIndex] is Consultant)
                    {
                        Consultant consultant = (Consultant)_employees[ComboBox1.SelectedIndex];
                        _users.Add(consultant.ChangeTo(SurnameTextBox.Text, NameTextBox.Text, PatronymicTextBox.Text, PhoneTextBox.Text, _users[ListBox1.SelectedIndex].Passport,
                            _users[ListBox1.SelectedIndex]));
                    }
                    else
                    {
                        Manager manager = (Manager)_employees[ComboBox1.SelectedIndex];
                        _users.Add(manager.ChangeTo(SurnameTextBox.Text, NameTextBox.Text, PatronymicTextBox.Text, PhoneTextBox.Text, PassportTextBox.Text,
                            _users[ListBox1.SelectedIndex]));
                    }

                    _users.RemoveAt(ListBox1.SelectedIndex);

                    using (StreamWriter sw = new StreamWriter(_path))
                    {
                        foreach (User user in _users)
                        {
                            sw.WriteLine(user.Surname + "~" + user.Name + "~" + user.Patronymic + "~" + user.PhoneNumber + "~" + user.Passport + "~" + user.ChangedDT + "~" + user.ChangedEmplpoyee);
                        }
                    }

                    FillUsersList();

                    CheckUser();
                    PhoneTextBox.Text = "";
                    LockAll();
                }
                catch
                {
                    MessageBox.Show("Операция не выполнена!");
                }
            }
            else
            {
                MessageBox.Show("Данные не были изменены!");
            }
        }

        /// <summary>
        /// Описание события при выборе элемента ListBox'a
        /// </summary>
        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox1.SelectedIndex >= 0)
            {
                if (_employees[ComboBox1.SelectedIndex] is Consultant)
                {
                    SurnameTextBox.IsEnabled = false;
                    NameTextBox.IsEnabled = false;
                    PatronymicTextBox.IsEnabled = false;
                    PhoneTextBox.IsEnabled = true;
                    PassportTextBox.IsEnabled = false;
                }
                else if (_employees[ComboBox1.SelectedIndex] is Manager)
                {
                    SurnameTextBox.IsEnabled = true;
                    NameTextBox.IsEnabled = true;
                    PatronymicTextBox.IsEnabled = true;
                    PhoneTextBox.IsEnabled = true;
                    PassportTextBox.IsEnabled = true;
                }
                else
                {
                    SurnameTextBox.IsEnabled = false;
                    NameTextBox.IsEnabled = false;
                    PatronymicTextBox.IsEnabled = false;
                    PhoneTextBox.IsEnabled = false;
                    PassportTextBox.IsEnabled = false;
                }

                SurnameTextBox.Text = _redactedUsers[ListBox1.SelectedIndex].Surname;
                NameTextBox.Text = _redactedUsers[ListBox1.SelectedIndex].Name;
                PatronymicTextBox.Text = _redactedUsers[ListBox1.SelectedIndex].Patronymic;
                PhoneTextBox.Text = _redactedUsers[ListBox1.SelectedIndex].PhoneNumber.ToString();
                PassportTextBox.Text = _redactedUsers[ListBox1.SelectedIndex].Passport;
            }
        }

        /// <summary>
        /// Заполнение списка работников из файла
        /// </summary>
        private void FillEmployees()
        {
            try
            {
                using (StreamReader sr = new StreamReader("EmployeesData.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] line = sr.ReadLine().Split('~');
                        switch (int.Parse(line[3]))
                        {
                            case 0:
                                _employees.Add(new Consultant(line[0], line[1], line[2]));
                                break;
                            case 1:
                                _employees.Add(new Manager(line[0], line[1], line[2]));
                                break;
                            default:

                                break;
                        }
                    }
                }

                foreach (Employee line in _employees)
                {
                    TextBlock textBox = new TextBlock();
                    textBox.Text = line.Surname + " " + line.Name + " " + line.Patronymic;
                    ComboBox1.Items.Add(textBox);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить список сотрудников!");
            }
        }

        /// <summary>
        /// Перезапись списка клиентов при изменении данных
        /// </summary>
        private void CheckUser()
        {
            _redactedUsers.Clear();
            foreach (User user in _users)
            {
                if (_employees[ComboBox1.SelectedIndex] is Consultant)
                {
                    _redactedUsers.Add(new User(user.Surname, user.Name, user.Patronymic, user.PhoneNumber.ToString(), "**** ******", user.ChangedDT, user.ChangedEmplpoyee));
                }
                else
                {
                    _redactedUsers.Add(new User(user.Surname, user.Name, user.Patronymic, user.PhoneNumber.ToString(), user.Passport, user.ChangedDT, user.ChangedEmplpoyee));
                }
            }

            ListBox1.ItemsSource = null;
            ListBox1.ItemsSource = _redactedUsers;
        }

        /// <summary>
        /// Проверка на изменения в данных клиентов
        /// </summary>
        /// <returns>true если данные НЕ изменились, false если данные изменились</returns>
        private bool CheckOnConfirmClick()
        {
            try
            {
                User currentUser = _redactedUsers[ListBox1.SelectedIndex];

                return SurnameTextBox.Text.Equals(currentUser.Surname) &&
                       NameTextBox.Text.Equals(currentUser.Name) &&
                       PatronymicTextBox.Text.Equals(currentUser.Patronymic) &&
                       PhoneTextBox.Text.Equals(currentUser.PhoneNumber.ToString()) &&
                       PassportTextBox.Text.Equals(currentUser.Passport);
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Блокировка и очистка полей
        /// </summary>
        private void LockAll()
        {
            SurnameTextBox.IsEnabled = false;
            NameTextBox.IsEnabled = false;
            PatronymicTextBox.IsEnabled = false;
            PhoneTextBox.IsEnabled = false;
            PassportTextBox.IsEnabled = false;
            SurnameTextBox.Text = "";
            NameTextBox.Text = "";
            PatronymicTextBox.Text = "";
            PhoneTextBox.Text = "";
            PassportTextBox.Text = "";
        }
    }


}
