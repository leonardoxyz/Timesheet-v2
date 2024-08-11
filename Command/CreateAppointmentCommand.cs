using MediatR;
using Appointments.Models;
using static Appointment;

namespace Appointments.Commands
{
    public class CreateAppointmentCommand : IRequest<Result>
    {
        public Appointment Appointment { get; }

        public CreateAppointmentCommand(Appointment appointment)
        {
            Appointment = appointment;
        }
    }
}