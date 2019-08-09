using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.Logs;

namespace TasksManagerClient.Statics
{
    static class LogManager
    {
        public static readonly ILogger Logger; 

        static LogManager()
        {
            Logger = new MessageBoxLogger();
        }
    }
}
