using EduVault.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EduVault.Data
{
    public class PostgresDBContext : DbContext
    {
        public DbSet<AccessRights> AccessRights { get; set; }
        public DbSet<AccessRightsType> AccessRightsTypes { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<FileTypeField> FileTypeFields { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Relation> Relationships { get; set; }
        public DbSet<Models.Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public PostgresDBContext(DbContextOptions<PostgresDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessRights>(entity =>
            {
                entity.HasKey(ar => ar.Id);
                entity.Ignore(a => a.Recipient);
            });

            modelBuilder.Entity<AccessRightsType>(entity =>
            {
                entity.HasKey(art => art.Id);
            });

            modelBuilder.Entity<Field>(entity =>
            {
                entity.HasKey(field => field.Id);
            });

            modelBuilder.Entity<FileType>(entity =>
            {
                entity.HasKey(ft => ft.Id);
            });

            modelBuilder.Entity<FileTypeField>(entity =>
            {
                entity.HasKey(ftt => ftt.Id);
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.HasKey(record => record.Id);
            });

            modelBuilder.Entity<Relation>(entity =>
            {
                entity.HasKey(relation => relation.Id);
            });

            modelBuilder.Entity<Models.Group>(entity =>
            {
                entity.HasKey(group => group.Id);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);

                entity.Property(user => user.Id)
                    //.HasColumnName("User_id")
                    .UseIdentityAlwaysColumn()
                    .ValueGeneratedOnAdd();

                entity.Property(user => user.Login)
                    .HasColumnName("Login")
                    .HasColumnType("character varying")
                    .IsRequired();

                entity.Property(user => user.PasswordHash)
                    .HasColumnName("Password")
                    .HasColumnType("character varying")
                    .IsRequired();

                entity.Property(user => user.Name)
                    .HasColumnName("Name")
                    .HasColumnType("character varying")
                    .IsRequired(false); // NULL разрешен

                entity.Property(user => user.UserType)
                    .HasColumnName("UserType")
                    .HasColumnType("byte");

                entity.Property(user => user.RoleId)
                    .HasColumnName("RoleId")
                    .HasColumnType("integer");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(role => role.Id);
                entity.Property(role => role.Id)
                    .HasColumnName("Role_id")
                    .UseIdentityAlwaysColumn()
                    .ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<GroupUser>(entity =>
            {
                // Составной первичный ключ из UserId и GroupId
                entity.HasKey("UserId", "GroupId");

                // Настройка связи с User
                entity.HasOne(gu => gu.User)
                    .WithMany(u => u.Groups)
                    .HasForeignKey(gu => gu.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Удаление GroupUser при удалении User

                // Настройка связи с Group
                entity.HasOne(gu => gu.Group)
                    .WithMany(g => g.Users)
                    .HasForeignKey(gu => gu.GroupId)
                    .OnDelete(DeleteBehavior.Cascade); // Удаление GroupUser при удалении Group
            });
        }
    }
}
