namespace Appointments.Models.Dto
{
    public class AppointmentDto
    {
        public int TaskId { get; set; }
        public DateTime TimeEntry { get; set; }
        public DateTime ExitTime { get; set; }
        public string EmployeeType { get; set; }
        public int ProjectId { get; set; }
    }
}
