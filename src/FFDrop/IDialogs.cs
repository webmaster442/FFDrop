namespace FFDrop;

internal interface IDialogs
{
    bool ConfirmMessage(string message, string title);
    void ErrorMessage(string message, string title);
    void InfoMessage(string message, string title);
    string? SelectFolderDialog(string? startFolder);
    string? SaveFileDialog(string filterString);
    void WarningMessage(string message, string title);
    void TextDialog(string message, string title);
}