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

            //FirstExampleLinqVsExtensionMethod(ctx);

            //Restrictions(ctx);

            //Grouping(ctx);

            Joining(ctx);
                
        }

        static void FirstExampleLinqVsExtensionMethod(PlutoContext ctx)
        {
            //LINQ Syntax:
            var query =
                from c in ctx.Courses
                where c.Title.Contains("c#")
                orderby c.Title
                select c;

            foreach (var c in query)
                Console.WriteLine(c.Title);

            //Extension methods:
            var courses = ctx.Courses
                .Where(c => c.Title.Contains("c#"))
                .OrderBy(c => c.Title);

            foreach (var course in courses)
                Console.WriteLine(course.Title);
        }

        static void Restrictions (PlutoContext ctx)
        {
            var query =
                from c in ctx.Courses
                where c.Level == CourseLevel.Beginner && c.AuthorId == 1
                select new { Name = c.Title, Id = c.Id };
        }

        static void Grouping (PlutoContext ctx)
        {
            var query =
                from c in ctx.Courses
                group c by c.Level
                into g
                select g;

            foreach (var group in query)
            {
                Console.WriteLine(group.Key);

                //display courses and next to it's level - print the number of courses in each group
                Console.WriteLine($"{group.Count()}");

                foreach (var course in group)
                {
                    //display courses of each group (grouped by course level)
                    //Console.WriteLine("\t{0}", course.Title);
                }
            }
        }

        static void Joining (PlutoContext ctx)
        {
            // use navigation propertie
            var query =
                from c in ctx.Courses
                select new { CourseName = c.Title, CourseAuthor = c.Author };

            foreach (var c in query)
            {
                Console.WriteLine($"Course name: {c.CourseName.Substring(0, 9)} | course author: {c.CourseAuthor.Name}");
            }

            Console.WriteLine("-----------------------------------------------------------");

            // use join
            var query1 =
                from c in ctx.Courses
                join a in ctx.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Title, CourseAuthorName = a.Name };

            foreach (var c in query1)
            {
                Console.WriteLine($"Course name: { c.CourseName.Substring(0, 9) } | course author: { c.CourseAuthorName }");
            }
        }
    }
}
