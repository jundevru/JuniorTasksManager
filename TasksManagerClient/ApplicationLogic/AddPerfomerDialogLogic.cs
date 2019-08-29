using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Model;
using TasksManagerClient.Statics;
using TasksManagerClient.ViewModel;

namespace TasksManagerClient.ApplicationLogic
{
    class AddPerformerDialogLogic : IPageDialogLogic
    {
        public event Action<PageDialogResult> EndEvent;

        IPageDialogPresenter presenter;
        WorkTask currentTask;

        public AddPerformerDialogLogic(IPageDialogPresenter presenter, WorkTask task)
        {
            this.presenter = presenter;
            currentTask = task;
        }
        public void StartDialog()
        {
            SelectUserViewModel suvm = new SelectUserViewModel();
            suvm.UserResult += (result) =>
            {
                if (result)
                {
                    var user = suvm.SelectedUser;
                    if (currentTask.Performers.FirstOrDefault(p => p.User.ID == user.ID) != null)
                    {
                        MessageBox.Show("Пользователь " + user.FIO + " уже есть в списке исполнителей.");
                        presenter.ShowPage(suvm);
                    }
                    else
                    {
                        ResolutionAndDateViewModel rdvm = new ResolutionAndDateViewModel();
                        rdvm.PerfomerResult += (res) =>
                        {
                            if (res == PageDialogResult.Canceled)
                                EndEvent?.Invoke(res);
                            else
                            {
                                currentTask.Performers.Add(new Performer()
                                {
                                    User = user,
                                    Message = rdvm.Message,
                                    PeriodOfExecution = rdvm.Period,
                                    WorkTask = currentTask
                                });
                                if (DB.TaskDataBase.Instance.SafeSaveChanges())
                                    EndEvent?.Invoke(res);
                            }
                        };
                        presenter.ShowPage(rdvm);
                    }
                }
                else
                    EndEvent?.Invoke(PageDialogResult.Canceled);
            };
            presenter.ShowPage(suvm);           
        }
    }
}
