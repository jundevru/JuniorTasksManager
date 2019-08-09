using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.Dialogs;

namespace TasksManagerClient.ApplicationLogic
{
    class NewTaskPageDialogLogic : IPageDialogLogic
    {
        public event Action<PageDialogResult> EndEvent;
        private IPageDialogPresenter presenter;
        public NewTaskPageDialogLogic(IPageDialogPresenter presenter)
        {
            this.presenter = presenter;
        }
        public void StartDialog()
        {
            
        }
    }
}
