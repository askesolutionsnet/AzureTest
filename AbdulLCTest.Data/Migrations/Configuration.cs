namespace AbdulLCTest.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AbdulLCTest.Data.PostDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AbdulLCTest.Data.PostDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Posts.AddOrUpdate(
              p => p.Subject,
              new Posts { Subject = "Test Message 1", Description= "My First Message as admin",CreatedBy= "24cb48d7-bcd0-4dbe-9356-b9ab34e1d043",CreatedDate=DateTime.Now }, 
              new Posts { Subject = "Test Message 2", Description= "My Second Message by guest", CreatedBy = "97950fb9-4d0c-4eab-80e3-0631a1e5ed6d", CreatedDate = DateTime.Now }
            );

        }
    }
}
