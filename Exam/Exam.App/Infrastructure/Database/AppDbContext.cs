using Exam.App.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Vet> Vets { get; set; }
    public DbSet<AnimalSpecie> AnimalSpecies { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Assistant> Assistants { get; set; }
    public DbSet<Examination> Examinations { get; set; }
    public DbSet<ExamReport> ExamReports { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Owner>()
            .HasOne(o => o.User)
            .WithOne(o => o.Owner)
            .HasForeignKey<Owner>(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vet>()
           .HasOne(v => v.User)
           .WithOne(v => v.Vet)
           .HasForeignKey<Vet>(o => o.UserId)
           .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vet>()
           .HasMany(v => v.Examinations)
           .WithOne(v => v.Vet)
           .HasForeignKey(o => o.VetId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Assistant>()
           .HasOne(a => a.User)
           .WithOne(a => a.Assistant)
           .HasForeignKey<Assistant>(a => a.UserId)
           .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Vet)
            .WithMany(v => v.Patients)
            .HasForeignKey(p => p.VetId)
            .OnDelete(DeleteBehavior.Restrict);//ne moze se obrisati veterinar dok se ne obrisu svi povezani pacijenti

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Owner)
            .WithMany(o => o.Pats)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.AnimalSpecie)
            .WithMany()//ne mora u entitetu biti kolekcija da bi mogli da stavimo withmany, jednoj vrsti pripada vise pacijenata
            .HasForeignKey(p => p.AnimalSpecieId)
            .OnDelete(DeleteBehavior.Restrict);//nad seed podacima nikad ne ide cascade brisanje

        modelBuilder.Entity<Patient>()
            .HasMany(p => p.Examinations)
            .WithOne(p => p.Pet)
            .HasForeignKey(p => p.PetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Examination>()
            .HasOne(p => p.Report)
            .WithOne(p => p.Examination)
            .HasForeignKey<ExamReport>(p => p.ExaminationId)
            .OnDelete(DeleteBehavior.Restrict);


        // Seed Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "b1c4c3f8-4d2f-4e62-9a4b-8e3a2a9e8a01", Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
            new IdentityRole { Id = "1bfe3e46-2f8f-4a9c-bb7b-2f0d8c2e6d02", Name = "User", NormalizedName = "USER" },
            new IdentityRole { Id = "28eeaa9a-294c-4bbe-ad1a-80cea6b3a840", Name = "Vet", NormalizedName = "VET"},
            new IdentityRole { Id = "8557fdcb-f8fd-4350-9c00-e1aa6efbef91", Name = "Assistant", NormalizedName = "ASSISTANT" },
            new IdentityRole { Id = "c78ecb03-2488-4e28-b1ac-1055a22bc072", Name = "Owner", NormalizedName = "OWNER" }
        );

        // Seed Entities
        modelBuilder.Entity<AnimalSpecie>(e =>
        {
            e.HasData(
              new AnimalSpecie { Id = 1, Name = "Pas" },
              new AnimalSpecie { Id = 2, Name = "Mačka" },
              new AnimalSpecie { Id = 3, Name = "Papagaj" },
              new AnimalSpecie { Id = 4, Name = "Kornjača" },
              new AnimalSpecie { Id = 5, Name = "Zec" },
              new AnimalSpecie { Id = 6, Name = "Hrčak" }
            );
        });

    }
}
