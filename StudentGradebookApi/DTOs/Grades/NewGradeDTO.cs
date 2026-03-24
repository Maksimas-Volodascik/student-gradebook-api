namespace StudentGradebookApi.DTOs.Grades
{
    public class NewGradeDto
    {
        public byte score {  get; set; }
        public string gradeType { get; set; }
        public DateTime gradingDate {  get; set; }
        public int enrollmentId { get; set; }
    }
}
