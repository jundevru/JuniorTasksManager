using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TasksManagerClient.Model;
using TasksManagerClient.Dialogs;
using TasksManagerClient.ApplicationLogic;
using TasksManagerClient.Statics;

namespace TasksManagerClient.ViewModel
{
    class NewTaskViewModel : Helpers.Notifier, IPageDialog
    {
        public event Action<PageDialogResult> CreateResult;
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
            CreateResult?.Invoke(PageDialogResult.Canceled);
        },(obj)=> {return true; });

        public ICommand CreateCommand => new Helpers.CommandsDelegate((obj)=> 
        {        
            CreateResult?.Invoke(PageDialogResult.Completed);
        }, (obj)=> { return true; });

        public NewTaskViewModel()
        {
            NewTask = new WorkTask()
            {
                Message = "Введите описание задачи общее для всех исполнителей",
                Priority = WorkTaskPriority.Normal,
                Access = WorkTaskAccess.OnlyPerformers,
                User = CurrentUser.Instance.User,
                State = WorkTaskStates.Work
            };
        }

        public void UpdatePropertyes()
        {
            //
        }
    }
}
