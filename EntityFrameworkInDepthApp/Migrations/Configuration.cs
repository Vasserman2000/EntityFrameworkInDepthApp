namespace EntityFrameworkInDepthApp.Migrations
{
    using System;
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


            context.Authors.AddOrUpdate(a => a.Name,
                new Author
                {
                    Name = "Kalanit Efrat",
                    Courses = new Collection<Course>()
                    {
                        new Course() { Title = "Marketing Strategy", Description = "..." }
                    }
                },
                new Author
                {
                    Name = "Erez Rozenblit",
                    Courses = new Collection<Course>()
                    {
                        new Course() { Title = "Financial Accounting", Description = "..." }
                    }
                }
                );
        }
    }
}
