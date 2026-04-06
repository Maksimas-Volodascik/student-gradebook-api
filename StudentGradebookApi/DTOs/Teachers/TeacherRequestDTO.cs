namespace StudentGradebookApi.DTOs.Teachers
{
    public class TeacherRequestDTO
    {
        //DTO For 'Create Teacher' and 'Edit Teacher'
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int ClassSubjectId { get; set; } 
    }
}
