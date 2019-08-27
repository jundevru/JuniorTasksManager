using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TasksManagerClient.Model;

namespace TasksManagerClient.DB
{
    class TaskDataBase : DbContext
    {
        public static DateTime NullDate => new DateTime(1900, 01, 01);
        public static readonly TaskDataBase Instance = new TaskDataBase("HomeConnection");
        private TaskDataBase(string dbName)
            : base(dbName)
        {
            this.Set<WorkTask>().AsNoTracking();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Performer> Perfomers { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<UserGroup> Groups { get; set; }

        public bool SafeSaveChanges()
        {
            try
            {
                SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
            return false;
        }        
    }
}
