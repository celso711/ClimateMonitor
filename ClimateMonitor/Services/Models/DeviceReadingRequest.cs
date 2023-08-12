namespace ClimateMonitor.Services.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;




public class DeviceReadingRequest
{
    [Required]
    [FirmwareVersionValidation]
    public string FirmwareVersion { get; set; } = string.Empty;

    [Required]
    public decimal Temperature { get; set; }

    [Required]
    public decimal Humidity { get; set; }
}

public class FirmwareVersionValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var firmwareVersion = value as string;
        var pattern = @"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-((?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$";
        if (firmwareVersion != null && Regex.IsMatch(firmwareVersion, pattern))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("The firmware value does not match semantic versioning format.");
    }
}