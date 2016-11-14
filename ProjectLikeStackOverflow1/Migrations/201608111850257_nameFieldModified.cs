namespace ProjectLikeStackOverflow1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nameFieldModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Answer1", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "Answer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Answer", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "Answer1");
        }
    }
}
