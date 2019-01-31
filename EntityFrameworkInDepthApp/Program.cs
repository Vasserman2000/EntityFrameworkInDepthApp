using EntityFrameworkInDepthApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkInDepthApp
{
    public class PlutoContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Course>()
                .Property(t => t.Title)
                .IsRequired();

        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            PlutoContext ctx = new PlutoContext();
            ctx.Authors.Add(new Author() { Name = "Yosssi", Courses = new List<Course>() { new Course() { Title = "a" } } });
            ctx.SaveChanges();
            ctx.Authors.FirstOrDefault();
        }
    }
}
