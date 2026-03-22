namespace StudentGradebookApi.DTOs.Grades
{
    public class GradesListDto
    {
        public int Score { get; set; }
        public string Grade_Type { get; set; }
        public DateTimeOffset GradingDate { get; set; }
    }
}
