using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TasksManagerClient.Model;

namespace TasksManagerClient.ViewModel
{
    class SelectUserViewModel : Helpers.Notifier, TasksManagerClient.Dialogs.IPageDialog
    {
        public string Title => "Выберите исполнителя";

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                RaisePropertyChanged();
            }
        }

        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                RaisePropertyChanged();
            }
        }

        public ICommand NextCommand => new Helpers.CommandsDelegate((obj) =>
        {

        }, (obj) => { return true; });

        public ICommand BackCommand => new Helpers.CommandsDelegate((obj) =>
        {

        }, (obj) => { return true; });

        public SelectUserViewModel()
        {
            try
            {
                DB.TaskDataBase.Instance.Users.Load();
                Users = DB.TaskDataBase.Instance.Users.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
