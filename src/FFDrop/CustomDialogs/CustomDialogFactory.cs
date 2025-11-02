using FFDrop.Model;

namespace FFDrop.CustomDialogs;

internal static class CustomDialogFactory
{
    public static ICustomDialog  Create(Dialogdefinition dialogdefinition)
    {
        return dialogdefinition.Dialogtype switch
        {
            Dialogtype.Selector => CreateSelector(dialogdefinition),
            Dialogtype.Time => CreateTimeDialog(dialogdefinition),
            _ => throw new NotSupportedException($"Dialog type {dialogdefinition.Dialogtype} is not supported."),
        };
    }

    private static ICustomDialog CreateSelector(Dialogdefinition dialogdefinition)
    {
        var dialog = new SelectorDialogWindow(dialogdefinition.Values ?? Array.Empty<string>(), dialogdefinition.DefaultValue ?? string.Empty)
        {
            Title = dialogdefinition.Title,
            Description = dialogdefinition.Description,
            Owner = App.Current.MainWindow,
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
        };
        return dialog;
    }

    private static ICustomDialog CreateTimeDialog(Dialogdefinition dialogdefinition)
    {
        var dialog = new TimeSelectorDoalogWindow()
        {
            Title = dialogdefinition.Title,
            Owner = App.Current.MainWindow,
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
        };
        return dialog;
    }
}
