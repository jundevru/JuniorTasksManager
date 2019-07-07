using System;
using System.Windows.Forms;

namespace TasksManagerServer
{
    static class InvokeHelper
    {
        public static void InvokeEx(this Form control, Action metod)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(metod);
            else
                metod();
        }
    }
}
