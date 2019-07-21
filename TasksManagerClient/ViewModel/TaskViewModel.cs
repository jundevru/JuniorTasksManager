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
    class TaskViewModel : Helpers.Notifier, Dialogs.IPageDialog
    {
        private ObservableCollection<WorkTask> tasks;
        /// <summary>
        /// Список задач
        /// </summary>
        public ObservableCollection<WorkTask> WorkTasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                RaisePropertyChanged();
            }

        }

        private WorkTask currentTask;
        /// <summary>
        /// Текущая выбранная задача
        /// </summary>
        public WorkTask CurrentTask
        {
            get { return currentTask; }
            set
            {
                currentTask = value;
                RaisePropertyChanged();
            }
        }

        #region ICommands
        public ICommand NewTaskCommand => new Helpers.CommandsDelegate((obj)=>
        {

        },(obj)=> { return true; });
        #endregion

        public ICommand EditTaskCommand => new Helpers.CommandsDelegate((obj) =>
        {

        }, (obj) => { return CurrentTask != null; });

        public string Title => "Список задач";
        private Dialogs.IPageDialogPresenter presenter;

        public TaskViewModel(Dialogs.IPageDialogPresenter presenter)
        {
            this.presenter = presenter;
            //UpdateTasks();
        }
        private void UpdateTasks()
        {
            try
            {
                throw new NotImplementedException("111");
                //DB.TaskDataBase.Instance.WorkTasks.Where((t) => t.User.ID == "Выбрать по текущему юзеру" || t.Performers.FirstOrDefault(p=> p.User.ID == "Выбрать по текущему юзеру") != null).Load();
                //WorkTasks = DB.TaskDataBase.Instance.WorkTasks.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки списка задач: " + ex.Message);
            }
        }

    }
}
