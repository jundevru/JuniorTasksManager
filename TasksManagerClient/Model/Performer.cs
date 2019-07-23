using System;
using System.ComponentModel;
using TasksManagerClient.Helpers;

namespace TasksManagerClient.Model
{
    enum WorkTaskStates
    {
        [Description ("В работе")]
        Work,
        [Description ("Исполнено")]
        Complette,
        [Description ("Отменено")]
        Cancel
    }
    /// <summary>
    /// Определяет единичного исполнителя конкретной задачи 
    /// </summary>
    class Performer : Notifier
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

        private WorkTaskStates state;
        /// <summary>
        /// Текущий статус исполнения
        /// </summary>
        public WorkTaskStates State
        {
            get { return state; }
            set
            {
                state = value;
                RaisePropertyChanged();
            }
        }

        private string message;
        /// <summary>
        /// Информация о ходе исполнения
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

        private DateTime perOfExe;
        /// <summary>
        /// Установленный срок исполнения
        /// </summary>
        public DateTime PeriodOfExecution
        {
            get { return perOfExe; }
            set
            {
                perOfExe = value;
                RaisePropertyChanged();
            }
        }




        /// <summary>
        /// Пользователь назначенный исполнителем
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Задача данного исполнителя
        /// </summary>
        public virtual WorkTask WorkTask { get; set; }

    }
}
