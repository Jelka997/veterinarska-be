namespace Exam.App.Domain
{
    public class ExamReport
    {
        public int Id { get; set; }
        public string Anamnesis { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public double PatientWeight { get; set; }
        public Examination Examination { get; set; }
        public int ExaminationId { get; set; }

        public bool CanUpdate()
        {
            int workingDays = 0;

            var limitDate = CreatedAt.Date;

            while (workingDays < 3)
            {
                limitDate = limitDate.AddDays(1);

                if (limitDate.DayOfWeek != DayOfWeek.Saturday &&
                    limitDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            return DateTime.Now.Date <= limitDate;
        }
    }
}
