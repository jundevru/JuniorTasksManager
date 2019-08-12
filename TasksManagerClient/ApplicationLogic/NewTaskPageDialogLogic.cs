using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Model;
using TasksManagerClient.Statics;
using TasksManagerClient.ViewModel;

namespace TasksManagerClient.ApplicationLogic
{
    class NewTaskPageDialogLogic : IPageDialogLogic
    {
        public event Action<PageDialogResult> EndEvent;
        private IPageDialogPresenter presenter;
        private WorkTask newTask;

        public NewTaskPageDialogLogic(IPageDialogPresenter presenter)
        {
            this.presenter = presenter;
        }
        public void StartDialog()
        {
            NewTaskViewModel ntwm = new NewTaskViewModel();
            ntwm.CreateResult += (res) =>
            {
                if (res == PageDialogResult.Canceled)
                    EndEvent?.Invoke(res);
                else
                {
                    newTask = ntwm.NewTask;
                    ResolutionAndDateViewModel rdvm = new ResolutionAndDateViewModel();
                    rdvm.PerfomerResult += (result) =>
                    {
                        if (result == PageDialogResult.Canceled)
                            EndEvent?.Invoke(result);
                        else
                        {
                            newTask.Performers = new List<Performer>();
                            newTask.Performers.Add(new Performer()
                            {
                                User = CurrentUser.Instance.User,
                                Message = rdvm.Message,
                                PeriodOfExecution = rdvm.Period,
                                WorkTask = newTask
                            });
                            DB.TaskDataBase.Instance.WorkTasks.Add(newTask);
                            if (DB.TaskDataBase.Instance.SafeSaveChanges())
                                EndEvent?.Invoke(result);
                        }
                    };
                    presenter.ShowPage(rdvm);
                }
            };
            presenter.ShowPage(ntwm);
        }
    }
}
