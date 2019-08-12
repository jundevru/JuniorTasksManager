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
using TasksManagerClient.Dialogs;
using TasksManagerClient.Statics;
using TasksManagerClient.ApplicationLogic;

namespace TasksManagerClient.ViewModel
{
    class TasksViewModel : Helpers.Notifier, IPageDialog, IUpdateSenderReceiver
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
        /// <summary>
        /// Новая задача
        /// </summary>
        public ICommand NewTaskCommand => new Helpers.CommandsDelegate((obj)=>
        {
            newTaskLogic.StartDialog();
        },(obj)=> { return true; });      
        /// <summary>
        /// Редактировать задачу
        /// </summary>
        public ICommand EditTaskCommand => new Helpers.CommandsDelegate((obj) =>
        {

        }, (obj) => { return CurrentTask != null; });
        #endregion

        #region Logic
        NewTaskPageDialogLogic newTaskLogic;
        #endregion

        public string Title => "Список задач";
        private IPageDialogPresenter presenter;

        public event Action ReceiveUpdate;

        public TasksViewModel(IPageDialogPresenter presenter)
        {
            this.presenter = presenter;
            newTaskLogic = new NewTaskPageDialogLogic(presenter);
            newTaskLogic.EndEvent += (res) => 
            {
                if (res == PageDialogResult.Completed)
                {
                    ReceiveUpdate?.Invoke();
                    UpdatePropertyes();
                }
                presenter.ShowPage(this);
            };
            UpdatePropertyes();
        }

        public void UpdatePropertyes()
        {
            try
            {
                DB.TaskDataBase.Instance.WorkTasks.Load();
                //.Where((t) => t.User.ID == CurrentUser.Instance.User.ID 
                //|| t.Performers.FirstOrDefault(p => p.User.ID == CurrentUser.Instance.User.ID) != null)
                WorkTasks = (ObservableCollection<WorkTask>)DB.TaskDataBase.Instance.WorkTasks.Local.Where((t) => t.User.ID == CurrentUser.Instance.User.ID
                || t.Performers.FirstOrDefault(p => p.User.ID == CurrentUser.Instance.User.ID) != null);
                MessageBox.Show(CurrentUser.Instance.User.ID + ", " +WorkTasks.Count);
            }
            catch (Exception ex)
            {
                LogManager.Logger.Write("Ошибка загрузки списка задач", ex);
            }
        }

        public void SendUpdate()
        {
            //
        }
    }
}
