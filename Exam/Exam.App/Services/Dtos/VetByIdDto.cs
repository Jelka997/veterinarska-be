namespace Exam.App.Services.Dtos
{
    public class VetByIdDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<PatientPreviewForVetDto> Patients { get; set; } = [];
        public List<ExaminationPreviewForVetDto> Examinations{ get; set; } = [];
    }
}
