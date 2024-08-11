using Appointments.Models;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Data
{
    public class Context : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Appointment>()
                .Property(a => a.IsApproved)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .Property(a => a.EmployeeType)
                .IsRequired();
        }
    }
}
