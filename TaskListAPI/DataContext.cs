using Persistance.DTOs;
using Persistance.Converters;
using Microsoft.EntityFrameworkCore;


namespace TaskListAPI
{
    public class DataContext : DbContext
    {
        public DataContext() : base()
        {
        }
        
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        
        public virtual DbSet<ItemDB> Item { get; set; }

        public virtual DbSet<AttendeeDB> Attendee { get; set; }

        public virtual DbSet<TaskListDB> TaskList { get; set; }

        public virtual DbSet<AttendanceDB> Attendance { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Data Source=SPENCER-WINDOWS;Initial Catalog=TaskListDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
