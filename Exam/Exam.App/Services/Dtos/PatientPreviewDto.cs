namespace Exam.App.Services.Dtos
{
    public class PatientPreviewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AnimalSpecie { get; set; }
        public string OwnerFullName { get; set; }
        public string VetFullName { get; set; }
    }
}
