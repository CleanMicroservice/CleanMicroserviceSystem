using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Validation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RequiredIfAttribute : ValidationAttribute
{
    #region Properties

    public string OtherProperty { get; }

    public string OtherPropertyDisplayName { get; set; }

    public object OtherPropertyValue { get; }

    public bool IsInverted { get; set; }

    public override bool RequiresValidationContext => true;

    #endregion Properties

    #region Constructor

    public RequiredIfAttribute(string otherProperty, object otherPropertyValue)
        : base("'{0}' is required because '{1}' has a value {3}'{2}'.")
    {
        this.OtherProperty = otherProperty;
        this.OtherPropertyValue = otherPropertyValue;
        this.IsInverted = false;
    }

    #endregion Constructor

    public override string FormatErrorMessage(string name)
    {
        return string.Format(
            CultureInfo.CurrentCulture,
            this.ErrorMessageString,
            name,
            this.OtherPropertyDisplayName ?? this.OtherProperty,
            this.OtherPropertyValue,
            this.IsInverted ? "other than " : "of ");
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext == null)
            throw new ArgumentNullException(nameof(validationContext));

        var otherProperty = validationContext.ObjectType.GetProperty(this.OtherProperty);
        if (otherProperty == null)
        {
            return new ValidationResult(
                string.Format(CultureInfo.CurrentCulture, "Could not find a property named '{0}'.", this.OtherProperty));
        }

        var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

        if ((!this.IsInverted && Equals(otherValue, this.OtherPropertyValue)) ||
            (this.IsInverted && !Equals(otherValue, this.OtherPropertyValue)))
        {
            if (value == null)
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));

            if (value is string val && val.Trim().Length == 0)
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }

        return ValidationResult.Success;
    }
}