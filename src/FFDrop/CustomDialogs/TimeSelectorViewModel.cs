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
    {
        ValidateProperty(value, nameof(FromTime));
        OnPropertyChanged(nameof(IsOkEnabled));
    }

    [ObservableProperty]
    [CustomValidation(typeof(TimeSelectorViewModel), nameof(ValidateToTime))]
    public partial string ToTime { get; set; }

   partial void OnToTimeChanged(string value)
    {
        ValidateProperty(value, nameof(ToTime));
        OnPropertyChanged(nameof(IsOkEnabled));
    }

    [ObservableProperty]
    public partial bool IsToEnabled { get; set; }

    public bool IsOkEnabled => !HasErrors;

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
            if (vm.IsToEnabled)
            {
                if (TimeRegex().IsMatch(value))
                {
                    if (TimeSpan.TryParse(vm.FromTime, out var from) &&
                        TimeSpan.TryParse(vm.ToTime, out var to)
                        && from <= to)
                    {
                        return ValidationResult.Success!;
                    }
                    return new ValidationResult("From time must be less or equal to to time");
                }
                else
                {
                    return new ValidationResult("Invalid time format. Please use HH:MM:SS or MM:SS. or seconds");
                }
            }
            else
            {
                return ValidationResult.Success!;
            }
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
