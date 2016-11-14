namespace ProjectLikeStackOverflow1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableForSessionAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionSaves",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NameOfSession = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SessionSaves");
        }
    }
}
