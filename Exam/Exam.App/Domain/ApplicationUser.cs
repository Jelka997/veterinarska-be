using Microsoft.AspNetCore.Identity;

namespace Exam.App.Domain;

public class ApplicationUser : IdentityUser
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public Owner? Owner { get; set; }
    public Vet? Vet { get; set; }
    public Assistant? Assistant { get; set; }
}