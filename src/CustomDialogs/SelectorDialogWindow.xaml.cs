using System.Windows;

namespace FFDrop.CustomDialogs
{
    /// <summary>
    /// Interaction logic for SelectorDialogWindow.xaml
    /// </summary>
    public partial class SelectorDialogWindow : Window, ICustomDialog
    {
        public SelectorDialogWindow(string[] items, string defaultselection)
        {
            InitializeComponent();
            LstOptions.ItemsSource = items;
            LstOptions.SelectedItem = defaultselection;
        }

        public string Description
        {
            get => TxtDescription.Text;
            set => TxtDescription.Text = value;
        }

        public string SelectedValue
            => LstOptions.SelectedItem as string ?? string.Empty;

        private void BtnOk_Click(object sender, RoutedEventArgs e)
            => DialogResult = true;

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
    }
}
