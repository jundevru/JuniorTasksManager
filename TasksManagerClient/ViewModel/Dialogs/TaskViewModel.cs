using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Model;

namespace TasksManagerClient.ViewModel
{
    class TaskViewModel : Helpers.Notifier, IPageDialog
    {
        public string Title => "Обзор задачи";

        private WorkTask workTask;
        public WorkTask WorkTask
        {
            get { return workTask; }
            set
            {
                workTask = value;
                RaisePropertyChanged();
            }
        }

        private Performer selectedPerformer;
        public Performer SelectedPerformer
        {
            get { return selectedPerformer; }
            set
            {
                selectedPerformer = value;
                RaisePropertyChanged();
            }
        }

        public ICommand AddPerfomerCommand => new Helpers.CommandsDelegate((obj) =>
        {

        }, (obj) => { return true; });

        public ICommand ToPerfomCommand => new Helpers.CommandsDelegate((obj) =>
        {

        }, (obj) => { return true; });

        public TaskViewModel(WorkTask task)
        {
            WorkTask = task;
        }
    }
}
