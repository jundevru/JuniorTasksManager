using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TasksManagerClient.ApplicationLogic;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Helpers;
using TasksManagerClient.Model;
using TasksManagerClient.Statics;

namespace TasksManagerClient.ViewModel.Dialogs
{
    class EditTaskViewModel : Notifier, IPageDialog
    {
        public string Title => "Просмотр/Редактирование задачи";
        public event Action<PageDialogResult> EditResult;

        private ObservableCollection<Performer> performers;
        public ObservableCollection<Performer> Performers
        {
            get { return performers; }
            set
            {
                performers = value;
                RaisePropertyChanged();
            }

        }

        private string completteComent;
        public string CompletteComent
        {
            get { return completteComent; }
            set
            {
                completteComent = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Отмена
        /// </summary>
        public ICommand CancelCommand => new CommandsDelegate((obj) => 
        {
            EditResult?.Invoke(PageDialogResult.Canceled);
        }, null);

        /// <summary>
        /// Завершить задачу
        /// </summary>
        public ICommand CompletteCommand => new CommandsDelegate((obj) =>
        {
            if (task.User.ID == CurrentUser.Instance.User.ID)
            {
                foreach (var item in task.Performers)
                    item.State = WorkTaskStates.Complette;
                task.State = WorkTaskStates.Complette;
                performer.AddMessage(CompletteComent);
            }
            else
            {
                if (performer != null)
                {
                    performer.State = WorkTaskStates.Complette;
                    performer.AddMessage(CompletteComent);
                }
                else
                {
                    MessageBox.Show("Не найден исполнитель, соответствующий данному пользователю. \n(Ошибка в модуле EditTaskViewModel, обратитесь к разработчику)");
                    EditResult?.Invoke(PageDialogResult.Canceled);
                    return;
                }
            }
            EditResult?.Invoke(DB.TaskDataBase.Instance.SafeSaveChanges() ? PageDialogResult.Completed : PageDialogResult.Canceled);
        }, (obj)=> { return !string.IsNullOrEmpty(CompletteComent) 
            && CurrentUser.Instance.User!=null 
            && performer != null
            && performer.State == WorkTaskStates.Work; });
        
        /// <summary>
        /// Отменить задачу
        /// </summary>
        public ICommand CancelTaskCommand => new CommandsDelegate((obj) =>
        {
            foreach (var item in task.Performers)
                item.State = WorkTaskStates.Cancel;
            task.State = WorkTaskStates.Cancel;
            performer.AddMessage(CompletteComent);
            EditResult?.Invoke(DB.TaskDataBase.Instance.SafeSaveChanges() ? PageDialogResult.Completed : PageDialogResult.Canceled);
        }, (obj) => {
            return !string.IsNullOrEmpty(CompletteComent)
               && CurrentUser.Instance.User != null
               && performer != null
               && task != null
               && CurrentUser.Instance.User.ID == task.User.ID
               && performer.State == WorkTaskStates.Work;
        });

        private WorkTask task;
        private Performer performer;

        public EditTaskViewModel(WorkTask task)
        {
            this.task = task;
            this.performer = task.Performers.FirstOrDefault(p => p.User.ID == CurrentUser.Instance.User.ID);
        }


        public void UpdatePropertyes()
        {
            if (task.User.ID == CurrentUser.Instance.User.ID)
                Performers = new ObservableCollection<Performer>(task.Performers);
            else
                Performers = new ObservableCollection<Performer>(task.Performers.Where(p=>p.User.ID == CurrentUser.Instance.User.ID).ToList());
        }
    }
}
