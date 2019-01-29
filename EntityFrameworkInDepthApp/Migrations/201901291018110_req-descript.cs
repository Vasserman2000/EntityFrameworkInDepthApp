namespace EntityFrameworkInDepthApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reqdescript : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "Description", c => c.String());
        }
    }
}
