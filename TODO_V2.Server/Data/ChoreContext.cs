//using BlazorBootstrap;
//using Microsoft.EntityFrameworkCore;
//using TODO_V2.Shared.Models;

//namespace TODO_V2.Server.Data
//{
//    public class ChoreContext: DbContext
//    {
//        public DbSet<Chore> Chores { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            //optionsBuilder.UseMySql("conexion",
//            //    new MySqlServerVersion(new Version(4, 9, 7)), options => options.EnableRetryOnFailure())
//            //    .LogTo(Console.WriteLine, LogLevel.Information)
//            //    .EnableSensitiveDataLogging()
//            //    .EnableDetailedErrors();
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<Chore>(entity =>
//            {
//                entity.HasKey(e => e.Id);
//                entity.Property(e => e.Registro);
//            });
//        }
//    }
//}
