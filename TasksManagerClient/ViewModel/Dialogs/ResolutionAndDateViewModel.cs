using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TasksManagerClient.ViewModel
{
    class ResolutionAndDateViewModel : Helpers.Notifier, Dialogs.IPageDialog
    {
        public string Title => "Укажите резолюцию и срок исполнения";


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

        }, (obj) => true);

        public ResolutionAndDateViewModel()
        {
            Period = DateTime.Now;
        }

        public void UpdatePropertyes()
        {
            throw new NotImplementedException();
        }
    }
}
