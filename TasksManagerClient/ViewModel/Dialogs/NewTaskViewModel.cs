using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TasksManagerClient.Model;
using TasksManagerClient.Dialogs;

namespace TasksManagerClient.ViewModel
{
    class NewTaskViewModel : Helpers.Notifier, IPageDialog
    {
        public string Title => "Новая задача";

        private WorkTask newTask;
        public WorkTask NewTask
        {
            get { return newTask; }
            set
            {
                newTask = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CancelCreateCommand => new Helpers.CommandsDelegate((obj)=> 
        {

        },(obj)=> {return true; });

        public ICommand CreateCommand => new Helpers.CommandsDelegate((obj)=> 
        {
            // Сохранить, добавив себя как первого исполнителя
            // Если задача не личная, перейти к добавлению прочих исполнителей - окно редактирования
        }, (obj)=> { return true; });

        public NewTaskViewModel()
        {
            NewTask = new WorkTask()
            {
                Message = "Введите текст задачи",
                Priority = WorkTaskPriority.Normal,
                Access = WorkTaskAccess.OnlyPerformers
            };
        }
    }
}
