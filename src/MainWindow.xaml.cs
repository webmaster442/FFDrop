using FFDrop.Model;
using System.Windows;

namespace FFDrop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(new Dialogs());
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
}