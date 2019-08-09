using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.Model;

namespace TasksManagerClient.Statics
{
    class CurrentUser : Helpers.Notifier
    {

        private User user;
        public User User
        {
            get { return user; }
            set
            {
                user = value;
                RaisePropertyChanged();
            }
        }

        public static CurrentUser Instance { get; private set; }
        static CurrentUser()
        {
            Instance = new CurrentUser();
        }
    }
}
