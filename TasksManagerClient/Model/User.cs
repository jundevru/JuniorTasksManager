using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.Helpers;

namespace TasksManagerClient.Model
{
    class User : Notifier
    {
        private int id;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                RaisePropertyChanged();
            }
        }

        private string login;
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                RaisePropertyChanged();
            }
        }

        private string passwordHash;
        /// <summary>
        /// Hash пароля для сверки
        /// </summary>
        public string PasswordHash
        {
            get { return passwordHash; }
            set
            {
                passwordHash = value;
                RaisePropertyChanged();
            }
        }

        private string fio;
        /// <summary>
        /// Уточняющие реквизиты пользователя
        /// </summary>
        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                RaisePropertyChanged();
            }
        }

        public virtual UserGroup Group { get; set; }

    }
}
