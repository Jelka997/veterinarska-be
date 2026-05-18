using Exam.App.Domain;
using System.ComponentModel.DataAnnotations;

namespace Exam.App.Services.Dtos
{
    public class CreatePatientDto
    {
        public string Name { get; set; }
        public int AnimalSpecieId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string OwnerUsername { get; set; }
    }
}
