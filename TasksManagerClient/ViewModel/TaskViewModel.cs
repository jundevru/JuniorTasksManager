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


        public string Title => "Список задач";
        private Dialogs.IPageDialogPresenter presenter;

        public TaskViewModel(Dialogs.IPageDialogPresenter presenter)
        {
            this.presenter = presenter;
        }
        private void UpdateTasks()
        {
            try
            {
                DB.TaskDataBase.Instance.WorkTasks.Where((t) => t.User.ID == "Выбрать по текущему юзеру").Load();
                WorkTasks = DB.TaskDataBase.Instance.WorkTasks.Local;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки списка задач: " + ex.Message);
            }
        }

    }
}
