namespace ProjectLikeStackOverflow1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AcceptedAnswerFieldAddedInQuestionModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "AcceptedAnswer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "AcceptedAnswer");
        }
    }
}
