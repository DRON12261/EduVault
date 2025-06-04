using EduVault.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace EduVault.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<AccessRights> AccessRights { get; set; }
		public DbSet<AccessRightsType> AccessRightsTypes { get; set; }
		public DbSet<Field> Fields { get; set; }
		public DbSet<FileType> FileTypes { get; set; }
		public DbSet<FileTypeField> FileTypeFields { get; set; }
		public DbSet<Record> Records { get; set; }
		public DbSet<Relation> Relationships { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
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

			modelBuilder.Entity<Role>(entity =>
			{
				entity.HasKey(role => role.Id);
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(user => user.Id);

				entity.Property(user => user.Id)
					.HasColumnName("user_id")
					.UseIdentityAlwaysColumn();
				entity.Property(user => user.Login)
					.HasColumnName("login")
					.HasColumnType("character varying")
					.IsRequired();

				entity.Property(user => user.PasswordHash)
					.HasColumnName("password")
					.HasColumnType("character varying")
					.IsRequired();

				entity.Property(user => user.Name)
					.HasColumnName("name")
					.HasColumnType("character varying")
					.IsRequired(false); // NULL разрешен

				entity.Property(user => user.UserType)
					.HasColumnName("usertype")
					.HasColumnType("bigint");
			});
		}
	}
}
