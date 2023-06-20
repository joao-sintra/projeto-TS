namespace Servidor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Auths", "Password", c => c.Binary(storeType: "binary(64)"));
            AlterColumn("dbo.Auths", "Salt", c => c.Binary(storeType: "binary(64)"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Auths", "Salt", c => c.String(maxLength: 64));
            AlterColumn("dbo.Auths", "Password", c => c.String(maxLength: 64));
        }
    }
}
