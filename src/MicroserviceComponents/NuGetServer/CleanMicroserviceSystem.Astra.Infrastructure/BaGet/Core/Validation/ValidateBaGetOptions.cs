using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Validation;

public class ValidateBaGetOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    private readonly string _optionsName;

    public ValidateBaGetOptions(string optionsName)
    {
        this._optionsName = optionsName;
    }

    public ValidateOptionsResult Validate(string name, TOptions options)
    {
        var context = new ValidationContext(options);
        var validationResults = new List<ValidationResult>();
        if (Validator.TryValidateObject(options, context, validationResults, validateAllProperties: true))
            return ValidateOptionsResult.Success;

        var errors = new List<string>();
        var message = this._optionsName == null
            ? $"Invalid configs"
            : $"Invalid '{this._optionsName}' configs";

        foreach (var result in validationResults)
        {
            errors.Add($"{message}: {result}");
        }

        return ValidateOptionsResult.Fail(errors);
    }
}