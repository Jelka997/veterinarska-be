using Exam.App.Domain;

namespace Exam.App.Services.Dtos
{
    public class ExaminationDto
    {
        public DateTime ExaminationDate { get; set; }
        public int VetId { get; set; }
        public int PetId { get; set; }
    }
}
