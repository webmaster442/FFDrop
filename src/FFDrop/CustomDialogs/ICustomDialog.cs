namespace FFDrop.CustomDialogs;

internal interface ICustomDialog
{
    string Title { get; set; }
    string Description { get; set; }
    string SelectedValue { get; }
    bool? ShowDialog();

    public bool TryGetCommandLineValue(out string commandLine)
    {
        if (ShowDialog() == true)
        {
            commandLine = SelectedValue;
            return true;
        }
        commandLine = string.Empty;
        return false;
    }
}
