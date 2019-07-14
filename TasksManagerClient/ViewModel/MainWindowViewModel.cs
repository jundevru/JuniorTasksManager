using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.DB;

namespace TasksManagerClient.ViewModel
{
    class MainWindowViewModel
    {
        
        public MainWindowViewModel()
        {
            TasksDataBase.Instance.Users.Add(new Model.User() {
                Login = "1",
                PasswordHash = "2",
                FIO = "3"
            });
            //TasksDataBase.Instance.SaveChanges();
        }
    }
}
