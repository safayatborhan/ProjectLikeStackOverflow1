namespace ProjectLikeStackOverflow1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredFieldAdded : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Answers", "Answer", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Answers", "Answer", c => c.String());
        }
    }
}
