using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksManagerClient.Model;

namespace TasksManagerClient.DB
{
    class TasksDataBase : DbContext
    {
        public static readonly TasksDataBase Instance = new TasksDataBase("HomeConnection");
        TasksDataBase(string dbName)
            : base(dbName)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Performer> Perfomers { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }

    }
}
