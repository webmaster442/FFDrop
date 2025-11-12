using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using CommunityToolkit.Mvvm.ComponentModel;

namespace FFDrop.CustomDialogs;

public partial class TimeSelectorViewModel: ObservableValidator
{
    [ObservableProperty]
    [CustomValidation(typeof(TimeSelectorViewModel), nameof(ValidateTime))]
    public partial string FromTime { get; set; }

    partial void OnFromTimeChanging(string value) => ValidateProperty(value);

    [ObservableProperty]
    [CustomValidation(typeof(TimeSelectorViewModel), nameof(ValidateTime))]
    public partial string ToTime { get; set; }

    partial void OnFromTimeChanged(string value) => ValidateProperty(value);

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

    public static ValidationResult ValidateTime(string value, ValidationContext context)
    {
        if (value == null || TimeRegex().IsMatch(value))
            return ValidationResult.Success!;

        return new ValidationResult("Invalid time format. Please use HH:MM:SS or MM:SS. or seconds");
    }

    [GeneratedRegex("^(?:(?:\\d+:\\d{2}:\\d{2}(?:\\.\\d+)?|\\d{1,2}:\\d{2}(?:\\.\\d+)?|\\d+(?:\\.\\d+)?))$")]
    private static partial Regex TimeRegex();
}
