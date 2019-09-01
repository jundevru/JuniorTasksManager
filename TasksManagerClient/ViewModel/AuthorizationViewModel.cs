using System;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity;
using TasksManagerClient.Dialogs;

namespace TasksManagerClient.ViewModel
{
    class AuthorizationViewModel : DependencyObject, IPageDialog
    {
        public string Title => "Авторизация";

        public event Action RegistrationRequiredEvent;
        public event Action<Model.User> AuthorizationEndEvent;

        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public static readonly DependencyProperty LoginProperty =
            DependencyProperty.Register("Login", typeof(string), typeof(AuthorizationViewModel), new PropertyMetadata(""));


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(AuthorizationViewModel), new PropertyMetadata(""));

        public ICommand Authorize => new Helpers.CommandsDelegate((obj) => {
            Password = (obj as PasswordBox).Password;
            Model.User user = null;
            try
            {
                user = DB.TaskDataBase.Instance.Users.Include(t=>t.Group).ToList().FirstOrDefault((u) => u.Login == Login);
            }
            catch(Exception ex)
            {
                Statics.LogManager.Logger.Write("Ошибка загрузки пользователей", ex);
                return;
            }             
            if (user == null)
            {
                MessageBox.Show("Пользователь с таким логином не найден в базе");
                return;
            }
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = Helpers.Utilits.GetHashString(Password);
                if (DB.TaskDataBase.Instance.SafeSaveChanges())
                    return;
                MessageBox.Show("Новый пароль успешно задан.");
            }
            else if (!user.PasswordHash.Equals(Helpers.Utilits.GetHashString(Password)))
            {
                MessageBox.Show("Не верная пара логин-пароль");
                return;
            }
            AuthorizationEndEvent?.Invoke(user);
        }, (obj) => {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty((obj as PasswordBox).Password);
        });

        public ICommand Registration => new Helpers.CommandsDelegate((obj) => {
            RegistrationRequiredEvent?.Invoke();
        }, (obj) => { return true; });

        public void UpdatePropertyes()
        {
            //
        }
    }
}
