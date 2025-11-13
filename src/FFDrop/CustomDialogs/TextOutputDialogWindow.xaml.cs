using System.Windows;

namespace FFDrop.CustomDialogs;
/// <summary>
/// Interaction logic for TextOutputDialog.xaml
/// </summary>
public partial class TextOutputDialogWindow : Window
{
    public TextOutputDialogWindow()
    {
        InitializeComponent();
    }

    public string TextOutput
    {
        get => TxtOutput.Text;
        set => TxtOutput.Text = value;
    }

    private void Ok_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
