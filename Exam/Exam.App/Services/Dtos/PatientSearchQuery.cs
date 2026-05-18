namespace Exam.App.Services.Dtos
{
    public class PatientSearchQuery
    {
        public string? VetName {  get; set; }
        public string? PatName { get; set; }
        public string? AnimalSpecie { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
    }
}
