using System;
using TasksManagerClient.Dialogs;
using TasksManagerClient.ViewModel;

namespace TasksManagerClient.ApplicationLogic
{
    class StartTurnOffPageDialogLogic : IPageDialogLogic
    {
        public event Action<PageDialogResult> EndEvent;

        private IPageDialogPresenter presenter;
        private AuthorizationViewModel avm;
        private RegistrationViewModel rvm = null;

        public StartTurnOffPageDialogLogic(IPageDialogPresenter presenter)
        {
            this.presenter = presenter;
            avm = new AuthorizationViewModel();
            avm.AuthorizationEndEvent += (user) =>
            {
                Statics.CurrentUser.Instance.User = user;
                EndEvent?.Invoke(PageDialogResult.Completed);
            };
            avm.RegistrationRequiredEvent += () =>
            {
                if (rvm == null)
                {
                    rvm = new RegistrationViewModel();
                    rvm.CancelEvent += () =>
                    {
                        presenter.ShowPage(avm);
                    };
                    rvm.RegistrationCompleteEvent += () =>
                    {
                        presenter.ShowPage(avm);
                    };
                }
                presenter.ShowPage(rvm);
            };           
        }

        public void StartDialog()
        {
            presenter.ShowPage(avm);
        }
    }
}
