using System.Collections.Generic;
using TasksManagerClient.Helpers;

namespace TasksManagerClient.Model
{

    enum WorkTaskPriority
    {
        Normal,
        High
    }
    enum WorkTaskAccess
    {
        OnlyMy,
        OnlyPerformers
    }
    class WorkTask : Notifier
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

        private string message;
        /// <summary>
        /// Информация о задаче
        /// </summary>
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged();
            }
        }

        private WorkTaskPriority priority;
        /// <summary>
        /// Приоритет задачи
        /// </summary>
        public WorkTaskPriority Priority
        {
            get { return priority; }
            set
            {
                priority = value;
                RaisePropertyChanged();
            }
        }

        private WorkTaskAccess access;
        /// <summary>
        /// Доступность задачи
        /// </summary>
        public WorkTaskAccess Access
        {
            get { return access; }
            set
            {
                access = value;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// Автор задачи
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Список исполнителей задачи
        /// </summary>
        public virtual ICollection<Performer> Performers { get; set; }
    }
}
