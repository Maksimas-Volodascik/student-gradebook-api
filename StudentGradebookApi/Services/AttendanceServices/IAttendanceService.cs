using StudentGradebookApi.DTOs.Attendances;
using StudentGradebookApi.Models;

namespace StudentGradebookApi.Services.AttendanceServices
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAllAttendances(AttendanceRequestDto attendanceRequest);
        Task<Attendance> GetAttendanceById(int id);
        Task CreateAttendance(NewAttendanceDto newAttendance);
        Task UpdateAttendance(NewAttendanceDto newAttendance, int id);
        Task DeleteAttendance(int id);
    }
}
