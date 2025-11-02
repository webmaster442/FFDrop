namespace FFDrop.Utils.Playlists;

public class PLSPlaylist : Playlist
{
    public override IEnumerable<string> Load(TextReader stringReader)
    {
        string? line;
        while ((line = stringReader.ReadLine()) != null)
        {
            line = line.Trim();
            if (line.StartsWith("File"))
            {
                var parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    yield return parts[1];
                }
            }
        }
    }

    public override void Save(IEnumerable<string> items, TextWriter stringWriter)
    {
        stringWriter.WriteLine("[playlist]");
        int index = 1;
        foreach (var item in items)
        {
            stringWriter.WriteLine($"File{index}={item}");
            index++;
        }
    }
}