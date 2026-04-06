using StudentGradebookApi.DTOs.Attendances;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.AttendancesRepository;

namespace StudentGradebookApi.Services.AttendanceServices
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendancesRepository _attendancesRepository;

        public AttendanceService(IAttendancesRepository attendancesRepository)
        {
            _attendancesRepository = attendancesRepository;
        }

        public Task CreateAttendance(NewAttendanceDto newAttendance)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAttendance(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Attendance>> GetAllAttendances(AttendanceRequestDto attendanceRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Attendance> GetAttendanceById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAttendance(NewAttendanceDto newAttendance, int id)
        {
            throw new NotImplementedException();
        }
    }
}
