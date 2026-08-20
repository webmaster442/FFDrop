using System.Diagnostics;
using System.Windows;

using FFDrop.Model;
using FFDrop.Tools;

namespace FFDrop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Dialogs _dialogs;

    public MainWindow()
    {
        InitializeComponent();
        _dialogs = new();
        DataContext = new MainWindowViewModel(_dialogs);
    }

    private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is MainWindowViewModel vm
            && e.NewValue is PresetViewModel preset)
        {
            vm.SelectedPreset = preset;
        }
    }

    private void Window_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
        else
            e.Effects = DragDropEffects.None;

        e.Handled = true;
    }

    private void Window_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effects = DragDropEffects.Copy;
        else
            e.Effects = DragDropEffects.None;

        e.Handled = true;
    }

    private void Window_Drop(object sender, DragEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm
            && e.Data.GetData(DataFormats.FileDrop) is string[] files)
        {
            vm.HandleDrop(files);
        }
    }

    private void MenuExit_Click(object sender, RoutedEventArgs e)
        => Close();

    private void MenuFFmpeg_Click(object sender, RoutedEventArgs e)
    {
        using var ffmpegWindow = new GithubDownloadWindow(_dialogs);
        ffmpegWindow.Owner = this;
        bool? result = ffmpegWindow.ShowDialog();
        if (result == true)
        {
            _dialogs.InfoMessage("FFMpeg successuflly downloaded", "Download complete");
        }
        else
        {
            _dialogs.WarningMessage("FFMpeg download cancelled or failed", "Download incomplete");
        }
    }

    private void MenuWebsite_Click(object sender, RoutedEventArgs e)
        => Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/webmaster442/FFDrop",
            UseShellExecute = true
        });
}