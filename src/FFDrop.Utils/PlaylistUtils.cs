using System.Diagnostics.CodeAnalysis;
using System.Text;

using FFDrop.Utils.Playlists;

namespace FFDrop.Utils;
public static class PlaylistUtils
{
    public static bool TryLoadPlaylistItems(string playlistPath, [NotNullWhen(true)] out IList<string>? items)
    {
        string extension = Path.GetExtension(playlistPath).ToLowerInvariant();
        Playlist? playlist = extension switch
        {
            ".m3u" or ".m3u8" => new M3UPlaylist(),
            ".pls" => new PLSPlaylist(),
            _ => null,
        };

        if (playlist == null)
        {
            items = null;
            return false;
        }

        using (var reader = new StreamReader(playlistPath, Encoding.UTF8, true))
        {
            items = playlist.Load(reader).ToList();
            return true;
        }
    }
}
