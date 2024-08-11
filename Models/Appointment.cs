using Appointments.Models;

public class Appointment : Entity
{
    public int TaskId { get; private set; }
    public DateTime TimeEntry { get; private set; }
    public DateTime ExitTime { get; private set; }
    public bool IsApproved { get; private set; }
    public string EmployeeType { get; private set; }
    public string RejectReason { get; private set; } = string.Empty;
    public int ProjectId { get; private set; }

    public Appointment(int taskId, DateTime timeEntry, DateTime exitTime, string employeeType, int projectId)
    {
        TaskId = taskId;
        TimeEntry = timeEntry;
        ExitTime = exitTime;
        EmployeeType = employeeType;
        IsApproved = false;
        RejectReason = string.Empty;
        ProjectId = projectId;
    }

    public void Approve()
    {
        IsApproved = true;
        RejectReason = null;
    }

    public void Reject(string reason)
    {
        IsApproved = false;
        RejectReason = reason;
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private Result(bool isSuccess, string errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true);
        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }
}