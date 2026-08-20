namespace FFDrop.Utils.Playlists;

public static class Extensions
{
    extension (Playlist)
    {
        public static void SaveM3u(IEnumerable<string> items, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            var m3uPlaylist = new M3UPlaylist();

            var relativePaths = items.Select(item => Path.GetRelativePath(Path.GetDirectoryName(filePath) ?? string.Empty, item));

            m3uPlaylist.Save(relativePaths, writer);
        }

        public static void SavePls(IEnumerable<string> items, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            var plsPlaylist = new PLSPlaylist();

            var relativePaths = items.Select(item => Path.GetRelativePath(Path.GetDirectoryName(filePath) ?? string.Empty, item));
            
            plsPlaylist.Save(relativePaths, writer);
        }
    }
}