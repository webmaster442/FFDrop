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
        var dialog = new SelectorDialogWindow(dialogdefinition.Values ?? Array.Empty<string>(), dialogdefinition.DefaultValue ?? string.Empty);
        dialog.Title = dialogdefinition.Title;
        dialog.Description = dialogdefinition.Description;
        return dialog;
    }

    private static ICustomDialog CreateTimeDialog(Dialogdefinition dialogdefinition)
    {
        throw new NotImplementedException();
    }
}
