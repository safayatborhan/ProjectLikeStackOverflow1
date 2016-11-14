namespace ProjectLikeStackOverflow1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserNameFieldAddedInAnswerModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "UserName");
        }
    }
}
