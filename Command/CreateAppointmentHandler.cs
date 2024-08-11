using Appointments.Commands;
using Appointments.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static Appointment;

public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, Result>
{
    private readonly AppointmentRepository _appointmentRepository;
    private readonly RulesService _rulesService;

    public CreateAppointmentHandler(AppointmentRepository appointmentRepository, RulesService rulesService)
    {
        _appointmentRepository = appointmentRepository;
        _rulesService = rulesService;
    }

    public async Task<Result> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Appointment;

        var appointment = new Appointment(
            dto.TaskId,
            dto.TimeEntry,
            dto.ExitTime,
            dto.EmployeeType,
            dto.ProjectId
        );

        _rulesService.ApplyRules(appointment);

        await _appointmentRepository.AddAsync(appointment);

        return Result.Success();
    }
}
