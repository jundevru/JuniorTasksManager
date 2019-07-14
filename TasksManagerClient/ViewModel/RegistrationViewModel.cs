using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TasksManagerClient.ViewModel
{
    class RegistrationViewModel : DependencyObject, Dialogs.IPageDialog
    {
        public string Title => "Регистрация";

        public event Action RegistrationCompleteEvent;
        public event Action CancelEvent;

        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Login", typeof(string), typeof(RegistrationViewModel), new PropertyMetadata(""));

        public string FIO
        {
            get { return (string)GetValue(FIOProperty); }
            set { SetValue(FIOProperty, value); }
        }
        public static readonly DependencyProperty FIOProperty =
            DependencyProperty.Register("FIO", typeof(string), typeof(RegistrationViewModel), new PropertyMetadata(""));

        public ICommand Registration => new Helpers.CommandsDelegate((obj) =>
        {
            string password = (obj as PasswordBox).Password;
            Model.User user = DB.TasksDataBase.Instance.Users.ToList().FirstOrDefault((u) => u.Login == Login);
            if (user != null)
            {
                MessageBox.Show("Данный логин занят");
                return;
            }
            MessageBox.Show(DB.TasksDataBase.Instance.Users.Local.Count + " " + Login);
            if (!Helpers.Utilits.PasswordIsValid(password))
            {
                MessageBox.Show("Пароль должен состоять из символов латинского алфавита, больших и маленьких и содержать цифру.");
                return;
            }
            user = new Model.User()
            {
                Login = Login,
                FIO = FIO,
                PasswordHash = Helpers.Utilits.GetHashString(password)
            };
            try
            {
                DB.TasksDataBase.Instance.Users.Add(user);
                DB.TasksDataBase.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Регистрация успешно завершена, пройдите авторизацию.");
            RegistrationCompleteEvent?.Invoke();
        }, (obj) =>
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(FIO) && !string.IsNullOrEmpty((obj as PasswordBox).Password);
        });


        public ICommand BackCancel => new Helpers.CommandsDelegate((obj) =>
        {
            CancelEvent?.Invoke();
        }, (obj) => 
        {
            return true;
        });


    }
}
