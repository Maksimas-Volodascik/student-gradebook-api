namespace StudentGradebookApi.Shared
{
    public class Errors //string Code, string? Field, string Message
    {
        public static class GenericErrors
        {
            public static readonly Error InternalServerError = new("internal.server.error", null, "Something went wrong. Please try again later.");
        }

        public static class ClassSubjectErrors
        {
            public static readonly Error ClassSubjectNotFound = new("class.subject.notFound", null, "ClassSubject not found.");
            public static readonly Error InvalidClassSubject = new("class.subject.invalid", "classSubject", "Class or Subject does not exist.");
        }

        public static class ClassesErrors
        {
            public static readonly Error ClassNotFound = new("class.not.found", null, "Class not found.");
            public static readonly Error InvalidClassesRoom = new("room.out.of.range", "room", "Invalid room number.");
            public static readonly Error InvalidClassesAcademicYear = new("academic.year.invalid.format", "academicYear", "Invalid academic year.");
        }

        public static class StudentErrors
        {
            public static readonly Error StudentNotFound = new("student.not.found", null, "Student not found.");
            public static readonly Error StudentDataNull = new("student.data.null", "studentData", "Student data cannot be null.");
            public static readonly Error StudentFirstNameEmpty = new("student.first.name.empty", "firstName", "Student first name cannot be empty.");
            public static readonly Error StudentLastNameEmpty = new("student.last.name.empty", "lastName", "Student last name cannot be empty.");
        }

        public static class EnrollmentErrors
        {
            //tba
        }

        public static class GradeErrors
        {
            public static readonly Error ScoreOutOfRange = new("score.out.of.range", "score", "Score number must be from 0 to 10.");
            public static readonly Error GradeTypeInvalid = new("grade.type.invalid", "gradeType", "Grade type must be one of the following: default, test, exam, project.");
            public static readonly Error GradingDateInvalid = new("grading.date.invalid", "gradingDate", "Grading date must be a valid date.");
            public static readonly Error EnrollmentIdInvalid = new("enrollment.id.invalid", "enrollmentId", "Enrollment ID must be a positive number.");
            public static readonly Error GradeNotFound = new("grade.not.found", null, "Grade not found.");
        }

        public static class SubjectErrors
        {
            public static readonly Error SubjectNameMissing = new("subject.name.missing", "name", "Subject name is required.");
            public static readonly Error SubjectCodeMissing = new("subject.code.missing", "code", "Subject code is required.");
            public static readonly Error SubjectNotFound = new("subject.not.found", null, "Subject not found.");
        }

        public static class TeacherErrors
        {
            public static readonly Error FirstNameMissing = new("teacher.firstname.missing", "firstName", "First name is required.");
            public static readonly Error LastNameMissing = new("teacher.lastname.missing", "lastName", "Last name is required.");
            public static readonly Error TeacherNotFound = new("teacher.not.found", null, "Teacher not found.");
        }

        public static class UserErrors
        {
            public static readonly Error EmailExists = new ("user.email.exists", "email", "Email already exists.");
            public static readonly Error EmailInvalid = new ("user.email.invalid", "email", "Invalid email address format.");
            public static readonly Error EmailRequired = new("user.email.required", "email", "Email is required.");
            public static readonly Error PasswordRequired = new("user.password.required", "password", "Password is required.");
            public static readonly Error InvalidUserCredentials = new("auth.invalid.credentials", "password", "Invalid email or password");
            public static readonly Error UserNotFound = new("user.not.found", null, "User not found.");
        }
    }
}
