namespace Telekomunikacije.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulatePurposeClass : DbMigration
    {
        public override void Up()
        {
            Sql("insert into Purposes (Id,Type) values (1,'Results')");
            Sql("insert into Purposes (Id,Type) values (2,'Lectures')");
        }
        
        public override void Down()
        {
        }
    }
}
