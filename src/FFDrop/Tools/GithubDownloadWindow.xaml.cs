using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Windows;

using FFDrop.DomainServices;
using FFDrop.Model.Github;
using FFDrop.Properties;

namespace FFDrop.Tools;

/// <summary>
/// Interaction logic for GithubDownloadWindow.xaml
/// </summary>
internal sealed partial class GithubDownloadWindow : Window, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IDialogs _dialogs;
    private DateTime _lastupdate;
    private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(800);

    public GithubDownloadWindow(IDialogs dialogs)
    {
        InitializeComponent();
        _cancellationTokenSource = new CancellationTokenSource();
        _dialogs = dialogs;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }


    private void SetMax(double max)
    {
        Dispatcher.Invoke(() =>
        {
            ProgressBarDownload.Maximum = max;
        });
    }

    private void UpdateStatus(double value, string message)
    {
        if (DateTime.UtcNow - _lastupdate > _updateInterval)
            _lastupdate = DateTime.UtcNow;
        else
            return;

        Dispatcher.Invoke(() =>
        {
            if (value < 0)
            {
                ProgressBarDownload.IsIndeterminate = true;
            }
            else
            {
                ProgressBarDownload.IsIndeterminate = false;
                ProgressBarDownload.Value = value;
            }
            TextBlockStatus.Text = message;
        });
    }

    private async Task<bool?> DoDownloadAndUnzip(CancellationToken cancellationToken)
    {
        try
        {
            UpdateStatus(-1, "Starting download...");
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "FFDrop.Application");
            HttpResponseMessage jsonStatus = await client.GetAsync(Settings.Default.FfmpegDownloadUrl, cancellationToken);
            jsonStatus.EnsureSuccessStatusCode();

            UpdateStatus(0, "Getting releses...");
            string json = await jsonStatus.Content.ReadAsStringAsync(cancellationToken);
            GithubRelease[] releases = GithubReleaseParser.Parse(json);

            GithubRelease release = releases.OrderByDescending(x => x.PublishedAt).First();

            ReleaseAsset asset = release.Assets.First(x => x.Name.Contains("win64-gpl-shared.zip"));

            SetMax(asset.Size);
            UpdateStatus(0, $"Downloading {asset.BrowserDownloadUrl}");

            HttpResponseMessage zipStatus = await client.GetAsync(asset.BrowserDownloadUrl, cancellationToken);
            zipStatus.EnsureSuccessStatusCode();

            using var zipStream = await zipStatus.Content.ReadAsStreamAsync(cancellationToken);
            using var memoryStream = new MemoryStream(asset.Size);
            byte[] copyBuffer = ArrayPool<byte>.Shared.Rent(16 * 1024);
            int totalRead = 0;
            int bytesRead;

            while ((bytesRead = await zipStream.ReadAsync(copyBuffer, cancellationToken)) > 0)
            {
                await memoryStream.WriteAsync(copyBuffer.AsMemory(0, bytesRead), cancellationToken);
                totalRead += bytesRead;
                UpdateStatus(totalRead, $"Downloading {asset.Url} ({totalRead}/{asset.Size} bytes)");
                cancellationToken.ThrowIfCancellationRequested();
            }

            UpdateStatus(-1, "Preparing to extract");
            memoryStream.Seek(0, SeekOrigin.Begin);
            ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

            var files = archive.Entries.Where(e => e.Name.EndsWith(".exe") || e.Name.EndsWith(".dll")).ToList();

            UpdateStatus(0, "Extracting files");
            SetMax(files.Count);

            int extractedCount = 0;
            foreach (var file in files)
            {
                UpdateStatus(0, $"Extracting {file.Name}");
                var targetPath = Path.Combine(AppContext.BaseDirectory, file.Name);
                using var fileStream = File.Create(targetPath);
                using var stream = file.Open();
                await stream.CopyToAsync(fileStream, cancellationToken);
                ++extractedCount;
            }

            UpdateStatus(files.Count, "Finished extracting");

            return true;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        catch (Exception ex)
        {
            Dispatcher.Invoke(() => _dialogs.ErrorMessage(ex.Message, "Error"));
            return false;
        }

    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
        DialogResult = false;
        Close();
    }

    protected override async void OnContentRendered(EventArgs e)
    {
        BringIntoView();
        DialogResult = await DoDownloadAndUnzip(_cancellationTokenSource.Token);
        Close();
    }
}
