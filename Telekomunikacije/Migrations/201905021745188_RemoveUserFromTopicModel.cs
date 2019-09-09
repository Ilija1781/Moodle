namespace Telekomunikacije.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUserFromTopicModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Topics", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Topics", new[] { "User_Id" });
            DropColumn("dbo.Topics", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Topics", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Topics", "User_Id");
            AddForeignKey("dbo.Topics", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
