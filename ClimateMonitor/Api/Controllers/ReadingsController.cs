using Microsoft.AspNetCore.Mvc;
using ClimateMonitor.Services;
using ClimateMonitor.Services.Models;

namespace ClimateMonitor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReadingsController : ControllerBase
{
    private readonly DeviceSecretValidatorService _secretValidator;
    private readonly AlertService _alertService;

    public ReadingsController(
        DeviceSecretValidatorService secretValidator,
        AlertService alertService)
    {
        _secretValidator = secretValidator;
        _alertService = alertService;
    }

    [HttpPost("evaluate")]
    public ActionResult<IEnumerable<Alert>> EvaluateReading(
     [FromHeader(Name = "x-device-shared-secret")] string deviceSecret,
     [FromBody] DeviceReadingRequest deviceReadingRequest)
    {
        if (!_secretValidator.ValidateDeviceSecret(deviceSecret))
        {
            return Problem(
                detail: "Device secret is not within the valid range.",
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return Ok(_alertService.GetAlerts(deviceReadingRequest));
    }
}
