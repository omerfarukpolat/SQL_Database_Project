namespace UygulamaOdevi2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<CONFERENCE> CONFERENCEs { get; set; }
        public virtual DbSet<CONFERENCE_TAGS> CONFERENCE_TAGS { get; set; }
        public virtual DbSet<COUNTRY> COUNTRies { get; set; }
        public virtual DbSet<COUNTRY_CITY> COUNTRY_CITY { get; set; }
        public virtual DbSet<SUBMISSION> SUBMISSIONS { get; set; }
        public virtual DbSet<USER> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CONFERENCE>()
                .Property(e => e.ConfID)
                .IsUnicode(false);

            modelBuilder.Entity<CONFERENCE>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CONFERENCE>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<CONFERENCE>()
                .Property(e => e.WebSite)
                .IsUnicode(false);

            modelBuilder.Entity<CONFERENCE>()
                .HasMany(e => e.CONFERENCE_TAGS)
                .WithOptional(e => e.CONFERENCE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CONFERENCE>()
                .HasMany(e => e.SUBMISSIONS)
                .WithOptional(e => e.CONFERENCE)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CONFERENCE_TAGS>()
                .Property(e => e.ConfID)
                .IsUnicode(false);

            modelBuilder.Entity<CONFERENCE_TAGS>()
                .Property(e => e.Tag)
                .IsUnicode(false);

            modelBuilder.Entity<COUNTRY>()
                .Property(e => e.Country_Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<COUNTRY>()
                .Property(e => e.Country_Name)
                .IsUnicode(false);

            modelBuilder.Entity<COUNTRY>()
                .HasMany(e => e.COUNTRY_CITY)
                .WithOptional(e => e.COUNTRY)
                .WillCascadeOnDelete();

            modelBuilder.Entity<COUNTRY_CITY>()
                .Property(e => e.Country_Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<COUNTRY_CITY>()
                .Property(e => e.City_Name)
                .IsUnicode(false);

            modelBuilder.Entity<SUBMISSION>()
                .Property(e => e.ConfID)
                .IsUnicode(false);

            modelBuilder.Entity<SUBMISSION>()
                .HasMany(e => e.SUBMISSIONS1)
                .WithOptional(e => e.SUBMISSION1)
                .HasForeignKey(e => e.prevSubmissionID);

            modelBuilder.Entity<USER>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.CONFERENCEs)
                .WithOptional(e => e.USER)
                .HasForeignKey(e => e.CreatorUser);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.SUBMISSIONS)
                .WithOptional(e => e.USER)
                .WillCascadeOnDelete();
        }
    }
}
