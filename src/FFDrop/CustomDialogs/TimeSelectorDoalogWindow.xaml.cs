using System.Windows;

namespace FFDrop.CustomDialogs;
/// <summary>
/// Interaction logic for TimeSelectorDoalogWindow.xaml
/// </summary>
public partial class TimeSelectorDoalogWindow : Window, ICustomDialog
{
    public string Description
    {
        get => TxtDescription.Text;
        set => TxtDescription.Text = value;
    }

    public string SelectedValue
    {
        get
        {
            Dictionary<string, string> props = new();
            props["from"] = TbCutFrom.Text;
            if (CbCutTo.IsChecked == true)
            {
                props["to"] = TbCutTo.Text;
            }
            return System.Text.Json.JsonSerializer.Serialize(props);
        }
    }

    public TimeSelectorDoalogWindow()
    {
        InitializeComponent();
    }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
        => DialogResult = true;

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
        => DialogResult = false;
}
