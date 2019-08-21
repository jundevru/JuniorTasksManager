using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Model;
using TasksManagerClient.ViewModel.Dialogs;

namespace TasksManagerClient.ApplicationLogic
{
    class EditTaskDialogLogic : IPageDialogLogic
    {
        public event Action<PageDialogResult> EndEvent;

        IPageDialogPresenter presenter;
        WorkTask currentTask;

        public EditTaskDialogLogic(IPageDialogPresenter presenter, WorkTask task)
        {
            this.presenter = presenter;
            this.currentTask = task;
        }

        public void StartDialog()
        {
            EditTaskViewModel etvm = new EditTaskViewModel(currentTask);
            etvm.EditResult += (res) =>
            {
                EndEvent?.Invoke(res);
            };
            presenter.ShowPage(etvm);
        }
    }
}
