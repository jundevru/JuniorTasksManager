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
using TasksManagerClient.Helpers;

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


        private int expiriedCount;
        /// <summary>
        /// Просрочка
        /// </summary>
        public int ExpiriedCount
        {
            get { return expiriedCount; }
            set
            {
                expiriedCount = value;
                RaisePropertyChanged();
            }
        }

        private int comingCount;
        /// <summary>
        /// Почти просрочка
        /// </summary>
        public int ComingCount
        {
            get { return comingCount; }
            set
            {
                comingCount = value;
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
        /// Добавить исполнителя
        /// </summary>
        public ICommand AddPerfomer => new Helpers.CommandsDelegate((obj) =>
        {
            addPerfomerLogic = new AddPerformerDialogLogic(presenter, CurrentTask);
            addPerfomerLogic.EndEvent += EndDialogResult;
            addPerfomerLogic.StartDialog();
        }, (obj) => { return CurrentTask != null
            && CurrentUser.Instance.User != null
            && CurrentTask.State == WorkTaskStates.Work
            && CurrentTask.Access == WorkTaskAccess.OnlyPerformers
            && CurrentTask.User.ID == CurrentUser.Instance.User.ID; });

        /// <summary>
        /// Редактировать задачу, исполнителям
        /// </summary>
        public ICommand EditTaskCommand => new Helpers.CommandsDelegate((obj)=>         
        {
            editTaskLogic = new EditTaskDialogLogic(presenter, CurrentTask);
            editTaskLogic.EndEvent += EndDialogResult;
            editTaskLogic.StartDialog();
        },(obj)=> { return CurrentTask != null; });

        private WorkTaskStates stateFilter = WorkTaskStates.Work;
        private string stateFilterButtonText = EnumHelper.GetDescription(WorkTaskStates.Work);
        public string StateFilterButtonText
        {
            get { return stateFilterButtonText; }
            set
            {
                stateFilterButtonText = value;
                RaisePropertyChanged();
            }
        }
        public ICommand StateFilterCommand => new Helpers.CommandsDelegate((obj) =>
        {
            int state = (int)stateFilter;
            state++;
            if (state > 2)
                state = 0;
            stateFilter = (WorkTaskStates)state;
            StateFilterButtonText = EnumHelper.GetDescription(stateFilter);
            UpdatePropertyes();
        }, null);
        #endregion

        #region Logic
        NewTaskPageDialogLogic newTaskLogic;
        AddPerformerDialogLogic addPerfomerLogic;
        EditTaskDialogLogic editTaskLogic;
        #endregion

        public string Title => "Список задач";
        private IPageDialogPresenter presenter;

        public event Action ReceiveUpdate;

        public TasksViewModel(IPageDialogPresenter presenter)
        {
            this.presenter = presenter;
            newTaskLogic = new NewTaskPageDialogLogic(presenter);
            newTaskLogic.EndEvent += EndDialogResult;
            UpdatePropertyes();
        }

        private void EndDialogResult(PageDialogResult result)
        {
            if (result == PageDialogResult.Completed)
            {
                ReceiveUpdate?.Invoke();
                UpdatePropertyes();
            }
            presenter.ShowPage(this);
        }

        public void UpdatePropertyes()
        {
            //MessageBox.Show((DateTime.Now - DateTime.Parse("01.09.2019")).Days.ToString());
            try
            {
                DB.TaskDataBase.Instance.WorkTasks.Load();
                List<WorkTask> tasks = DB.TaskDataBase.Instance.WorkTasks.Where((t) => t.User.ID == CurrentUser.Instance.User.ID
                || t.Performers.FirstOrDefault(p => p.User.ID == CurrentUser.Instance.User.ID) != null)
                .Include(t=>t.Performers)   // Загрузка вложенных данных внутри типа WorkTask, в данном случае списка исполнителей. (По умолчанию virtual не грузятся)
                .ToList();
                ExpiriedCount = tasks.Aggregate(0, (acc, v) => { return acc + (Utilits.DateTimeToUgrency(v.PeriodOfExecution) == Ugrencys.Expiried && v.State == WorkTaskStates.Work ? 1 : 0); });
                ComingCount = tasks.Aggregate(0, (acc, v) => { return acc + (Utilits.DateTimeToUgrency(v.PeriodOfExecution) == Ugrencys.Coming && v.State == WorkTaskStates.Work ? 1 : 0); });
                WorkTasks = new ObservableCollection<WorkTask>(tasks.Where(t=> t.State == stateFilter));
            }
            catch (Exception ex)
            {
                LogManager.Logger.Write("Ошибка загрузки списка задач", ex);
            }
        }

        // Получена информация о необходимости обновить данные
        public void SendUpdate()
        {
            UpdatePropertyes();
        }
    }
}
