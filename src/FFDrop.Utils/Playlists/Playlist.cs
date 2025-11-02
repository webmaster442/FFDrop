namespace FFDrop.Utils.Playlists;

public abstract class Playlist
{
    public abstract IEnumerable<string> Load(TextReader stringReader);
    public abstract void Save(IEnumerable<string> items, TextWriter stringWriter);
}
