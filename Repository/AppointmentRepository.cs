using Appointments.Data;
using Appointments.Models;

namespace Appointments.Repository
{
    public class AppointmentRepository : Repository<Appointment>
    {
        public AppointmentRepository(Context context) : base(context)
        {
        }
    }
}
