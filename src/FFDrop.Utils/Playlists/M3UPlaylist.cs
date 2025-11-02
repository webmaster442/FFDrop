namespace FFDrop.Utils.Playlists;

public class M3UPlaylist : Playlist
{
    public override IEnumerable<string> Load(TextReader stringReader)
    {
        string? line;
        while ((line = stringReader.ReadLine()) != null)
        {
            line = line.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
                continue;

            yield return line;
        }
    }

    public override void Save(IEnumerable<string> items, TextWriter stringWriter)
    {
        stringWriter.WriteLine("#EXTM3U");
        foreach (var item in items)
        {
            stringWriter.WriteLine(item);
        }
    }
}
