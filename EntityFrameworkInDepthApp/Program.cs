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
        public PlutoContext() : base()
        {
            // this way we can turn lazy loading off even if the navigational property defined as public virtual:
            Configuration.LazyLoadingEnabled = false;
        }

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

            //Ordering(ctx);

            //Grouping(ctx);

            //Joining(ctx);

            //Projection(ctx);

            //Partitioning(ctx);

            //Operators(ctx);

            //Quantifying(ctx);

            //Aggregating(ctx);

            //IQueryableVsIEnumerable(ctx);

            LazyLoading(ctx);
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

        static void Restrictions(PlutoContext ctx)
        {
            var query =
                from c in ctx.Courses
                where c.Level == CourseLevel.Beginner && c.AuthorId == 1
                select new { Name = c.Title, Id = c.Id };

            // LINQ Extension Method:
            // get all courses in level 'Beginner':
            var courses = ctx.Courses.Where(c => c.Level == CourseLevel.Beginner);

        }

        static void Grouping(PlutoContext ctx)
        {
            // linq syntax:
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

            //linq ext. methods:
            var groups = ctx.Courses.GroupBy(c => c.Level);

            foreach (var g in groups)
            {
                Console.WriteLine("Key: " + g.Key);
                foreach (var course in g)
                {
                    Console.WriteLine(course.Title);
                }
            }
        }

        static void Joining(PlutoContext ctx)
        {
            #region Navigation Property
            var query =
                from c in ctx.Courses
                select new { CourseName = c.Title, CourseAuthor = c.Author };

            foreach (var c in query)
            {
                Console.WriteLine($"Course name: {c.CourseName.Substring(0, 9)} | course author: {c.CourseAuthor.Name}");
            }

            Console.WriteLine("-----------------------------------------------------------");
            #endregion

            #region INNER JOIN (LINQ Syntax)
            var query1 =
                from c in ctx.Courses
                join a in ctx.Authors on c.AuthorId equals a.Id
                select new { CourseName = c.Title, CourseAuthorName = a.Name };

            foreach (var c in query1)
            {
                Console.WriteLine($"Course name: { c.CourseName.Substring(0, 9) } | course author: { c.CourseAuthorName }");
            }

            Console.WriteLine("-----------------------------------------------------------");
            #endregion

            #region GROUP JOIN (LINQ Syntax)

            // group join (no equivalent in sql but useful when we do LEFT JOINs in sql)
            var query2 =
                from a in ctx.Authors
                join c in ctx.Courses on a.Id equals c.AuthorId into g
                select new { AuthorName = a.Name, Courses = g.Count() };

            foreach (var x in query2)
            {
                Console.WriteLine($"{x.AuthorName} ({x.Courses})");
            }

            Console.WriteLine("-----------------------------------------------------------");
            #endregion

            #region CROSS JOIN (LINQ Syntax)
            // cross join
            var query3 =
                from a in ctx.Authors
                from c in ctx.Courses
                select new { AuthorName = a.Name, CourseName = c.Title };

            foreach (var i in query3)
            {
                Console.WriteLine($"{i.AuthorName} - {i.CourseName}");
            }
            #endregion

            #region INNER JOIN (LINQ Extension Method)
            var joinResult = ctx.Courses.Join(ctx.Authors,
                c => c.AuthorId,
                a => a.Id,
                (Course, Author) => new
                    {
                        CourseName = Course.Title,
                        AuthorName = Author.Name
                    });
            #endregion

            #region GROUP JOIN (LINQ Extension Method)
            // useful when using for LEFT JOINS with AGGREGATE function

            var groupJoinResult = ctx.Authors
                .GroupJoin(ctx.Courses,
                    a => a.Id, 
                    c => c.AuthorId, 
                    (author, courses) => 
                        new {
                            AuthorName = author,
                            Courses = courses.Count()
                            });
            #endregion

            #region CROSS JOIN (LINQ Extension Method)

            var crossJoinResult = ctx.Authors
                .SelectMany(
                    a => ctx.Courses,  
                    (author, course) => 
                    new {
                        AuthorName = author.Name,
                        Course = course.Title
                        });
            #endregion
        }

        static void Ordering (PlutoContext ctx)
        {
            // using LINQ Extension methods:
            var courses = ctx.Courses
                .Where(c => c.Level == CourseLevel.Beginner)
                .OrderByDescending(c => c.Title)
                .ThenByDescending(c => c.Level);
        }

        static void Projection (PlutoContext ctx)
        {
            // using LINQ Extension methods:
            var courses = ctx.Courses
                .Where(c => c.Level == CourseLevel.Beginner)
                .OrderBy(c => c.Title)
                .Select(c => new { CourseName = c.Title, AuthorName = c.Author.Name });

            // list of lists
            var courses1 = ctx.Courses
                .Where(c => c.Level == CourseLevel.Beginner)
                .OrderBy(c => c.Title)
                .Select(c => c.Tags);

            // list of tags
            var tags = ctx.Courses
                .Where(c => c.Level == CourseLevel.Beginner)
                .OrderBy(c => c.Title)
                .SelectMany(c => c.Tags).Distinct();

            foreach (var tag in tags)
            {
                Console.WriteLine(tag.Name);
            }
        }

        static void Partitioning (PlutoContext ctx)
        {
            #region Exists only in LINQ Extension Methods
            // display courses in pages, the size of each page - 3
            var courses = ctx.Courses
                                .OrderBy(c => c.Id)
                                .Skip(3)
                                .Take(3);

            foreach (var c in courses)
            {
                Console.WriteLine($"Course: {c.Title}");
            }
            #endregion
        }

        static void Operators(PlutoContext ctx)
        {
            #region Exists only in LINQ Extension Methods
            ctx.Courses.OrderBy(c => c.Level).FirstOrDefault(c => c.FullPrice > 100);

            // the "Last" method is not for use with sql database:
            ctx.Courses.LastOrDefault();

            ctx.Courses.SingleOrDefault(c => c.Id == 1);
            #endregion
        }

        static void Quantifying(PlutoContext ctx)
        {
            #region Exists only in LINQ Extension Methods
            // find if "all" items in a list satisfy a criteria:
            bool allAbove10Dollars = ctx.Courses.All(c => c.FullPrice > 10);

            // find if "any" item in a list satisfies a criteria:
            bool anyCourseInBeginnerLevel = ctx.Courses.Any(c => c.Level == CourseLevel.Beginner);
            #endregion
        }

        static void Aggregating(PlutoContext ctx)
        {
            #region Exists only in LINQ Extension Methods
            var count = ctx.Courses.Where(c => c.Level == CourseLevel.Beginner).Count();

            ctx.Courses.Max(c => c.FullPrice);

            ctx.Courses.Min(c => c.FullPrice);

            ctx.Courses.Average(c => c.FullPrice);

            #endregion
        }

        static void IQueryableVsIEnumerable(PlutoContext ctx)
        {
            // IQueryable allows queries to be ---extended--- without being immediately executed
            // We are building here an expression tree: we [compose] queries, extend them and delay execution
            IQueryable<Course> courses = ctx.Courses
                            .Where(c => c.Level == CourseLevel.Beginner)
                            .OrderByDescending(c => c.Id)
                            .Select(c => c);
            // When we use IEnumerable, we cannot extend (compose) a query so it executes at every step:
            IEnumerable<Course> courses2 = ctx.Courses
                            .Where(c => c.Level == CourseLevel.Beginner)
                            .OrderByDescending(c => c.Id)
                            .Select(c => c);
        }

        static void LazyLoading(PlutoContext ctx)
        {
            // if a navigational property defined as public virtual then the EF will use LAZY LOADING
            // so it will postpone executing the query for bringing the relational data so instead
            // going once to the DB and bringing all related data it will go to the DB on demand only
            // when we access them
            // which will cause additional roundtrips to the DB
            // use Single, First, MAX to immediately execute a query
            // Use Lazy Loading to load main objects and load the related objects on demand
            var courses = ctx.Courses.Single(c => c.Level == CourseLevel.Intermediate);
        }
    }
}
