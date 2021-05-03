using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CertificationCenter.Models {
    public class ApplicationContext : IdentityDbContext<User> {
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<UserCertifications> UserCertifications { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
            
            builder.Entity("CertificationCenter.Models.User", b => {
                b.Property<string>("Id")
                    .HasColumnType("text");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("text");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.HasKey("Id");

                b.ToTable("Users");
            });

            builder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b => {
                b.Property<string>("Id")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)");

                b.HasKey("Id");

                b.HasData(
                    new IdentityRole {Name = "user", NormalizedName = "USER"},
                    new IdentityRole {Name = "admin", NormalizedName = "ADMIN"}
                );

                b.ToTable("Roles");
            });

            builder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b => {
                b.Property<string>("LoginProvider")
                    .HasColumnType("text");

                b.Property<string>("ProviderKey")
                    .HasColumnType("text");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("text");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("text");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.HasOne("CertificationCenter.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.ToTable("UserLogins");
            });

            builder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b => {
                b.Property<string>("UserId")
                    .HasColumnType("text");

                b.Property<string>("RoleId")
                    .HasColumnType("text");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CertificationCenter.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.ToTable("UserRoles");
            });

            builder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b => {
                b.Property<string>("UserId")
                    .HasColumnType("text");

                b.Property<string>("LoginProvider")
                    .HasColumnType("text");

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.Property<string>("Value")
                    .HasColumnType("text");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.HasOne("CertificationCenter.Models.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.ToTable("UserTokens");
            });

            builder.Entity<Certification>(b => {
                b.Property(e => e.Id)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.Name)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.Description)
                    .HasColumnType("text");

                b.Property(e => e.DatetimeStart)
                    .HasColumnType("timestamp");

                b.Property(e => e.DatetimeEnd)
                    .HasColumnType("timestamp");

                b.Property(e => e.IsActive)
                    .HasColumnType("boolean");

                b.HasKey("Id");

                b.ToTable("Certifications");
            });

            builder.Entity<UserCertifications>(b => {
                b.Property(e => e.UserId)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.CertificationId)
                    .HasColumnType("text")
                    .IsRequired();

                b.HasKey("UserId", "CertificationId");

                b.HasOne(p => p.Certification)
                    .WithMany()
                    .HasForeignKey("CertificationId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne(p => p.User)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.ToTable("UserCertifications");
            });

            builder.Entity<Question>(b => {
                b.Property(e => e.Id)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.CertificationId)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.QuestionString)
                    .HasColumnType("text")
                    .IsRequired();

                b.HasOne(p => p.Certification)
                    .WithMany()
                    .HasForeignKey("CertificationId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasKey("Id");

                b.ToTable("Questions");
            });

            builder.Entity<Answer>(b => {
                b.Property(e => e.Id)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.UserId)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.CertificationId)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.QuestionId)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.AnswerString)
                    .HasColumnType("text");

                b.Property(e => e.IsCorrect)
                    .HasColumnType("boolean");

                b.HasKey("Id");

                b.HasOne(p => p.User)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne(p => p.Certification)
                    .WithMany()
                    .HasForeignKey("CertificationId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne(p => p.Question)
                    .WithMany()
                    .HasForeignKey("QuestionId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.ToTable("Answers");
            });
        }
    }
}