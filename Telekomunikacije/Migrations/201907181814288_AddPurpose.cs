namespace Telekomunikacije.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurpose : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Purposes",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FileModels", "PurposeId", c => c.Byte(nullable: false));
            CreateIndex("dbo.FileModels", "PurposeId");
            AddForeignKey("dbo.FileModels", "PurposeId", "dbo.Purposes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileModels", "PurposeId", "dbo.Purposes");
            DropIndex("dbo.FileModels", new[] { "PurposeId" });
            DropColumn("dbo.FileModels", "PurposeId");
            DropTable("dbo.Purposes");
        }
    }
}
