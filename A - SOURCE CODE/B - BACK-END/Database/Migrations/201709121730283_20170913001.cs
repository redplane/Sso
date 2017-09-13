namespace DbModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170913001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Email = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                        PhotoUrl = c.String(),
                        Role = c.Int(nullable: false),
                        JoinedTime = c.Double(nullable: false),
                        LastModifiedTime = c.Double(),
                    })
                .PrimaryKey(t => t.Email);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatorEmail = c.String(maxLength: 128),
                        CreatedTime = c.Double(nullable: false),
                        LastModifiedTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.CreatorEmail)
                .Index(t => t.CreatorEmail);
            
            CreateTable(
                "dbo.CategoryInitialization",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        CreatorEmail = c.String(nullable: false, maxLength: 128),
                        CreatedTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.CreatorEmail })
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.CreatorEmail, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.CreatorEmail);
            
            CreateTable(
                "dbo.FavouriteCategory",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        FollowerEmail = c.String(nullable: false, maxLength: 128),
                        FollowedTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.FollowerEmail })
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.FollowerEmail, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.FollowerEmail);
            
            CreateTable(
                "dbo.PhotoCategorization",
                c => new
                    {
                        PhotoId = c.String(nullable: false, maxLength: 128),
                        CategoryId = c.Int(nullable: false),
                        CategorizedTime = c.Double(nullable: false),
                        LastModifiedTime = c.Double(),
                    })
                .PrimaryKey(t => new { t.PhotoId, t.CategoryId })
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Photo", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.PhotoId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Photo",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Url = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        CreatedTime = c.Double(nullable: false),
                        LastModifiedTime = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhotoThumbnail",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PhotoId = c.String(maxLength: 128),
                        Url = c.String(),
                        Width = c.Double(nullable: false),
                        Height = c.Double(nullable: false),
                        CreatedTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photo", t => t.PhotoId)
                .Index(t => t.PhotoId);
            
            CreateTable(
                "dbo.PhotoUpload",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhotoId = c.String(maxLength: 128),
                        UploaderEmail = c.String(maxLength: 128),
                        UploadedTime = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photo", t => t.PhotoId)
                .ForeignKey("dbo.Account", t => t.UploaderEmail)
                .Index(t => t.PhotoId)
                .Index(t => t.UploaderEmail);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhotoUpload", "UploaderEmail", "dbo.Account");
            DropForeignKey("dbo.PhotoUpload", "PhotoId", "dbo.Photo");
            DropForeignKey("dbo.PhotoThumbnail", "PhotoId", "dbo.Photo");
            DropForeignKey("dbo.PhotoCategorization", "PhotoId", "dbo.Photo");
            DropForeignKey("dbo.PhotoCategorization", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.FavouriteCategory", "FollowerEmail", "dbo.Account");
            DropForeignKey("dbo.FavouriteCategory", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.CategoryInitialization", "CreatorEmail", "dbo.Account");
            DropForeignKey("dbo.CategoryInitialization", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Category", "CreatorEmail", "dbo.Account");
            DropIndex("dbo.PhotoUpload", new[] { "UploaderEmail" });
            DropIndex("dbo.PhotoUpload", new[] { "PhotoId" });
            DropIndex("dbo.PhotoThumbnail", new[] { "PhotoId" });
            DropIndex("dbo.PhotoCategorization", new[] { "CategoryId" });
            DropIndex("dbo.PhotoCategorization", new[] { "PhotoId" });
            DropIndex("dbo.FavouriteCategory", new[] { "FollowerEmail" });
            DropIndex("dbo.FavouriteCategory", new[] { "CategoryId" });
            DropIndex("dbo.CategoryInitialization", new[] { "CreatorEmail" });
            DropIndex("dbo.CategoryInitialization", new[] { "CategoryId" });
            DropIndex("dbo.Category", new[] { "CreatorEmail" });
            DropTable("dbo.PhotoUpload");
            DropTable("dbo.PhotoThumbnail");
            DropTable("dbo.Photo");
            DropTable("dbo.PhotoCategorization");
            DropTable("dbo.FavouriteCategory");
            DropTable("dbo.CategoryInitialization");
            DropTable("dbo.Category");
            DropTable("dbo.Account");
        }
    }
}
