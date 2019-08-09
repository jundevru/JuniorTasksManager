using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Helpers;
using TasksManagerClient.Model;
using TasksManagerClient.Statics;

namespace TasksManagerClient.ViewModel
{
    class RegistrationViewModel : Notifier, IPageDialog
    {
        public string Title => "Регистрация";

        public event Action RegistrationCompleteEvent;
        public event Action CancelEvent;

        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                RaisePropertyChanged();
            }
        }

        private string fio;
        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<UserGroup> groups;
        public ObservableCollection<UserGroup> Groups
        {
            get { return groups; }
            set
            {
                groups = value;
                RaisePropertyChanged();
            }
        }

        private UserGroup selectedGroup;
        public UserGroup SelectedGroup
        {
            get { return selectedGroup; }
            set
            {
                selectedGroup = value;
                RaisePropertyChanged();
            }
        }

        private string groupText;
        public string GroupText
        {
            get { return groupText; }
            set
            {
                groupText = value;
                RaisePropertyChanged();
            }
        }

        public ICommand Registration => new Helpers.CommandsDelegate((obj) =>
        {
            string password = (obj as PasswordBox).Password;
            Model.User user = null;
            try
            {
                user = DB.TaskDataBase.Instance.Users.ToList().FirstOrDefault((u) => u.Login == Login);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки пользователей: " + ex.Message);
                return;
            }
            if (user != null)
            {
                MessageBox.Show("Данный логин занят");
                return;
            }            
            if (!Helpers.Utilits.PasswordIsValid(password))
            {
                MessageBox.Show("Пароль должен состоять из символов латинского алфавита, больших и маленьких и содержать цифру.");
                return;
            }
            if (SelectedGroup == null && string.IsNullOrEmpty(GroupText))
            {
                MessageBox.Show("Необходимо указать группу");
                return;
            }

            UserGroup group;
            if (!string.IsNullOrEmpty(GroupText))
            {
                var fGroup = DB.TaskDataBase.Instance.Groups.ToList().FirstOrDefault((g) => g.Name.Equals(GroupText));
                if (fGroup == null)
                {
                    group = new UserGroup() { Name = GroupText };
                    DB.TaskDataBase.Instance.Groups.Add(group);
                    if (!DB.TaskDataBase.Instance.SafeSaveChanges())
                        return;
                }
                else
                    group = fGroup;
            }
            else
                group = SelectedGroup;
            user = new Model.User()
            {
                Login = Login,
                FIO = FIO,
                PasswordHash = Helpers.Utilits.GetHashString(password),
                Group = group                
            };
            DB.TaskDataBase.Instance.Users.Add(user);
            if (!DB.TaskDataBase.Instance.SafeSaveChanges())
                return;
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

        public void UpdatePropertyes()
        {
            try
            {
                DB.TaskDataBase.Instance.Groups.Load();
                Groups = DB.TaskDataBase.Instance.Groups.Local;
            }
            catch(Exception ex)
            {
                LogManager.Logger.Write("Ошибка загрузки списка групп", ex);
                return;
            }
        }
    }
}
