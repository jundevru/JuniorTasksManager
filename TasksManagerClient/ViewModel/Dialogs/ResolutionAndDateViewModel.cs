using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TasksManagerClient.ApplicationLogic;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Helpers;

namespace TasksManagerClient.ViewModel
{
    class ResolutionAndDateViewModel : Notifier, IPageDialog
    {
        public string Title => "Укажите резолюцию и срок исполнения";

        public event Action<PageDialogResult> PerfomerResult;

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged();
            }
        }

        private DateTime period;
        public DateTime Period
        {
            get { return period; }
            set
            {
                period = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CompleteCommand => new Helpers.CommandsDelegate((obj) =>
        {
            PerfomerResult?.Invoke(PageDialogResult.Completed);
        }, (obj) => !string.IsNullOrEmpty(Message));
        public ICommand CancelCommand => new Helpers.CommandsDelegate((obj) =>
        {
            PerfomerResult?.Invoke(PageDialogResult.Canceled);
        }, (obj) => true);
        


        public ResolutionAndDateViewModel()
        {
            Period = DateTime.Now;
            Message = "Введите текст резолюции здесь";
        }

        public void UpdatePropertyes()
        {
            //
        }
    }
}
