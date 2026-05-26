using System.Security.Cryptography;

namespace Exam.App.Domain
{
    public class Examination
    {
        public int Id { get; set; }
        public DateTime ExaminationDate { get; set; }
        public int VetId { get; set; }
        public Vet Vet { get; set; }
        public int PetId { get; set; }
        public Patient Pet { get; set; }
        public ExaminationStatus Status { get; set; }
        public string? CancellationReason { get; set; }
        public ExamReport? Report { get; set; }
    }
}