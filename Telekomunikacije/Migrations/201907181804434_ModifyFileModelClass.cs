namespace Telekomunikacije.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyFileModelClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileModels", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileModels", "Description");
        }
    }
}
