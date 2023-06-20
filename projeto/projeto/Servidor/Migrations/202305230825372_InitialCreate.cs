namespace Servidor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 64),
                        Password = c.String(maxLength: 64),
                        Salt = c.String(maxLength: 64),
                        IsOnline = c.Boolean(),
                        LastAuthentication = c.DateTime(nullable: false),
                        AccoutCreation = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Auths");
        }
    }
}
