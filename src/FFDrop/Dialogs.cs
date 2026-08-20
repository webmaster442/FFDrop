using System.Windows;

using FFDrop.DomainServices;
using FFDrop.Utils;
using FFDrop.Utils.FFProbe;

using Microsoft.Win32;

namespace FFDrop;

internal class Dialogs : IDialogs
{
    public void ErrorMessage(string message, string title)
    {
        MessageBox.Show(message,
                        title,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error,
                        MessageBoxResult.OK,
                        MessageBoxOptions.DefaultDesktopOnly);
    }

    public bool ConfirmMessage(string message, string title)
    {
        var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly);
        return result == MessageBoxResult.Yes;
    }

    public void InfoMessage(string message, string title)
    {
        MessageBox.Show(message,
                        title,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information,
                        MessageBoxResult.OK,
                        MessageBoxOptions.DefaultDesktopOnly);
    }

    public void WarningMessage(string message, string title)
    {
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public string? SelectFolderDialog(string? startFolder)
    {
        var selector = new Microsoft.Win32.OpenFolderDialog();
        if (!string.IsNullOrWhiteSpace(startFolder))
            selector.DefaultDirectory = startFolder;
        if (selector.ShowDialog() == true)
        {
            return selector.FolderName;
        }
        return null;
    }

    public string? SaveFileDialog(string filterString)
    {
        var sfd = new Microsoft.Win32.SaveFileDialog
        {
            Filter = filterString
        };
        if (sfd.ShowDialog() == true)
        {
            return sfd.FileName;
        }
        return null;
    }

    public void TextDialog(string message, string title)
    {
        var dialog = new CustomDialogs.TextOutputDialogWindow
        {
            Owner = Application.Current.MainWindow,
            TextOutput = message,
            Title = title,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        dialog.ShowDialog();
    }

    public void ShowFileInfoDialog(MediaInfoModel mediaInfo)
    {
        var dialog = new CustomDialogs.FileInfoDialogWindow(mediaInfo)
        {
            Owner = Application.Current.MainWindow,
            Title = $"File Information",
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        dialog.ShowDialog();
    }

    public bool SelectMediaFile(string title, out string selectedFile)
    {
        OpenFileDialog dialog = new()
        {
            Filter = FileRecognizer.MakeFilterString(),
            Title = title
        };
        if (dialog.ShowDialog() == true)
        {
            selectedFile = dialog.FileName;
            return true;
        }
        selectedFile = string.Empty;
        return false;
    }
}
