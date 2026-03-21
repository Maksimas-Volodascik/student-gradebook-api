using System.ComponentModel.DataAnnotations;

namespace StudentGradebookApi.Models
{
    public class Grades
    {
        [Key]
        public int Id { get; set; }
        [Range(0, 10, ErrorMessage ="Score must be between 1 and 10")]
        public byte Score { get; set; }
        //[Required(ErrorMessage = "Select grade type - default, test, exam")]
        public string Grade_Type { get; set; } = "default";
        public DateTimeOffset GradingDate { get; set; } = DateTimeOffset.Now;
        public int EnrollmentId { get; set; }
        public Enrollments Enrollments { get; set; } = null!;
        public DateTime GradingDay { get; private set; } //Date with no time (computes from GradingDate)
    }
}
