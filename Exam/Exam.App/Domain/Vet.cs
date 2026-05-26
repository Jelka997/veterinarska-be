namespace Exam.App.Domain
{
    public class Vet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }  
        public List<Patient> Patients { get; set; } = [];
        public List<Examination> Examinations { get; set; } = [];
    }
}
