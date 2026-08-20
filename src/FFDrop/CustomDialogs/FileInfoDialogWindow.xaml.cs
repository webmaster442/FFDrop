using System.Collections.ObjectModel;
using System.Windows;

using FFDrop.Utils;
using FFDrop.Utils.FFProbe;

namespace FFDrop.CustomDialogs;
/// <summary>
/// Interaction logic for FileInfoDialogWindow.xaml
/// </summary>
public partial class FileInfoDialogWindow : Window
{
    public FileInfoDialogWindow(MediaInfoModel mediaInfo)
    {
        InitializeComponent();
        FileInfoTreeView.ItemsSource = new ObservableCollection<MediaInfoModel>([mediaInfo]);
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
