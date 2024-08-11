using Microsoft.AspNetCore.Mvc;
using MediatR;
using Appointments.Commands;
using Appointments.Models.Dto;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] Appointment appointment)
    {
        if (appointment == null)
            return BadRequest("Invalid appointment data.");

        var command = new CreateAppointmentCommand(appointment);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok("Appointment created successfully.");
        }
        else
        {
            return BadRequest(result.ErrorMessage);
        }
    }
}