using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using CommunityToolkit.Mvvm.ComponentModel;

namespace FFDrop.CustomDialogs;

public partial class TimeSelectorViewModel: ObservableValidator
{
    [ObservableProperty]
    [CustomValidation(typeof(TimeSelectorViewModel), nameof(ValidateFromTime))]
    public partial string FromTime { get; set; }

    partial void OnFromTimeChanged(string value)
        => ValidateProperty(value, nameof(FromTime));

    [ObservableProperty]
    [CustomValidation(typeof(TimeSelectorViewModel), nameof(ValidateToTime))]
    public partial string ToTime { get; set; }

   partial void OnToTimeChanged(string value)
        => ValidateProperty(value, nameof(ToTime));
    
    [ObservableProperty]
    public partial bool IsToEnabled { get; set; }

    partial void OnIsToEnabledChanged(bool value)
    {
        ToTime = value ? ToTime : string.Empty;
        ValidateAllProperties();
    }

    public TimeSelectorViewModel()
    {
        FromTime = "0";
        IsToEnabled = false;
        ToTime = string.Empty;
    }

    public static ValidationResult ValidateToTime(string value, ValidationContext context)
    {
        if (context.ObjectInstance is TimeSelectorViewModel vm)
        {
            return vm.IsToEnabled
                ? TimeRegex().IsMatch(value)
                    ? ValidationResult.Success!
                    : new ValidationResult("Invalid time format. Please use HH:MM:SS or MM:SS. or seconds")
                : ValidationResult.Success!;
        }
        return new ValidationResult("Invalid state");
    }

    public static ValidationResult ValidateFromTime(string value, ValidationContext context)
    {
        if (TimeRegex().IsMatch(value))
            return ValidationResult.Success!;

        return new ValidationResult("Invalid time format. Please use HH:MM:SS or MM:SS. or seconds");
    }

    [GeneratedRegex("^(?:(?:\\d+:\\d{2}:\\d{2}(?:\\.\\d+)?|\\d{1,2}:\\d{2}(?:\\.\\d+)?|\\d+(?:\\.\\d+)?))$")]
    private static partial Regex TimeRegex();
}
