namespace EntityFrameworkInDepthApp.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using EntityFrameworkInDepthApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<EntityFrameworkInDepthApp.PlutoContext>
    {
        public Configuration()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../App_Data"));
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EntityFrameworkInDepthApp.PlutoContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var tags = new Dictionary<string, Tag>
            {
                {"c#", new Tag {Id = 1, Name = "c#"}},
                {"angularjs", new Tag {Id = 2, Name = "angularjs"}},
                {"javascript", new Tag {Id = 3, Name = "javascript"}},
                {"nodejs", new Tag {Id = 4, Name = "nodejs"}},
                {"oop", new Tag {Id = 5, Name = "oop"}},
                {"linq", new Tag {Id = 6, Name = "linq"}},
            };

            foreach (var tag in tags.Values)
                context.Tags.AddOrUpdate(t => t.Id, tag);

     
            var authors = new List<Author>
            {
                new Author
                {
                    Id = 1,
                    Name = "Mosh Hamedani"
                },
                new Author
                {
                    Id = 2,
                    Name = "Anthony Alicea"
                },
                new Author
                {
                    Id = 3,
                    Name = "Eric Wise"
                },
                new Author
                {
                    Id = 4,
                    Name = "Tom Owsiak"
                },
                new Author
                {
                    Id = 5,
                    Name = "John Smith"
                }
            };

            foreach (var author in authors)
                context.Authors.AddOrUpdate(a => a.Id, author);

            var courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Title = "C# Basics",
                    AuthorId = 1,
                    FullPrice = 49,
                    Description = "Description for C# Basics",
                    Level = CourseLevel.Beginner,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    Id = 2,
                    Title = "C# Intermediate",
                    AuthorId = 1,
                    FullPrice = 49,
                    Description = "Description for C# Intermediate",
                    Level = CourseLevel.Intermediate,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"],
                        tags["oop"]
                    }
                },
                new Course
                {
                    Id = 3,
                    Title = "C# Advanced",
                    AuthorId = 1,
                    FullPrice = 69,
                    Description = "Description for C# Advanced",
                    Level = CourseLevel.Advanced,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    Id = 4,
                    Title = "Javascript: Understanding the Weird Parts",
                    AuthorId = 2,
                    FullPrice = 149,
                    Description = "Description for Javascript",
                    Level = CourseLevel.Intermediate,
                    Tags = new Collection<Tag>()
                    {
                        tags["javascript"]
                    }
                },
                new Course
                {
                    Id = 5,
                    Title = "Learn and Understand AngularJS",
                    AuthorId = 2,
                    FullPrice = 99,
                    Description = "Description for AngularJS",
                    Level = CourseLevel.Intermediate,
                    Tags = new Collection<Tag>()
                    {
                        tags["angularjs"]
                    }
                },
                new Course
                {
                    Id = 6,
                    Title = "Learn and Understand NodeJS",
                    AuthorId = 2,
                    FullPrice = 149,
                    Description = "Description for NodeJS",
                    Level = CourseLevel.Intermediate,
                    Tags = new Collection<Tag>()
                    {
                        tags["nodejs"]
                    }
                },
                new Course
                {
                    Id = 7,
                    Title = "Programming for Complete Beginners",
                    AuthorId = 3,
                    FullPrice = 45,
                    Description = "Description for Programming for Beginners",
                    Level = CourseLevel.Beginner,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    Id = 8,
                    Title = "A 16 Hour C# Course with Visual Studio 2013",
                    AuthorId = 4,
                    FullPrice = 150,
                    Description = "Description 16 Hour Course",
                    Level = CourseLevel.Beginner,
                    Tags = new Collection<Tag>()
                    {
                        tags["c#"]
                    }
                },
                new Course
                {
                    Id = 9,
                    Title = "Learn JavaScript Through Visual Studio 2013",
                    AuthorId = 4,
                    FullPrice = 20,
                    Description = "Description Learn Javascript",
                    Level = CourseLevel.Beginner,
                    Tags = new Collection<Tag>()
                    {
                        tags["javascript"]
                    }
                }
            };

            foreach (var course in courses)
                context.Courses.AddOrUpdate(c => c.Id, course);
        }
    }
}
