namespace YouJuku.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class age : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Age", c => c.String(nullable: false));
            DropColumn("dbo.Students", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Students", "Age");
        }
    }
}
