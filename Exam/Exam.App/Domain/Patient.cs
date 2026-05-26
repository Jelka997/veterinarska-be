using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Exam.App.Domain
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AnimalSpecieId { get; set; }
        public AnimalSpecie AnimalSpecie { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public int? VetId { get; set; }
        public Vet Vet { get; set; }
        public List<Examination> Examinations { get; set; } = [];
    }
}
