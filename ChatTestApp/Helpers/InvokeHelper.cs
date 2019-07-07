using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ChatTestApp.Helpers
{
    static class InvokeHelper
    {
        public static void InvokeEx(Action action)
        {
            Dispatcher dispatchObject = Application.Current.Dispatcher;
            dispatchObject?.BeginInvoke(action);
        }

    }
}
