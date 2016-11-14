namespace ProjectLikeStackOverflow1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdOfQuestions = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Vote = c.Int(nullable: false),
                        Answer = c.Int(nullable: false),
                        View = c.Int(nullable: false),
                        Title = c.String(),
                        Question = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
