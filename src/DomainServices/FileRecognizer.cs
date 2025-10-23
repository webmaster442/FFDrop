using System.IO;

namespace FFDrop.DomainServices;

internal static class FileRecognizer
{
    private readonly static HashSet<string> AudioFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        ".aac", ".ac3", ".aiff", ".alac",
        ".amr", ".ape", ".atrac", ".au",
        ".caf", ".dts", ".flac", ".gsm",
        ".m4a", ".m4b", ".mka", ".mlp",
        ".mp2", ".mp3", ".oga", ".opus",
        ".ra", ".raw", ".shn", ".tak",
        ".tta", ".voc", ".wav", ".wma",
        ".wv"
    };

    private readonly static HashSet<string> VideoFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        ".3gp", ".asf", ".avi", ".bfi",
        ".f4v", ".flv", ".gxf", ".m4v",
        ".matroska", ".mkv", ".mov", ".mp4",
        ".mpeg", ".mpg", ".mts", ".mxf",
        ".nut", ".ogg", ".ogv", ".rm",
        ".ts", ".vob", ".webm", ".wm",
        ".wmv", ".yuv"
    };

    public static bool IsDropConvertSupported(string file)
    {
        var extension = Path.GetExtension(file);
        return AudioFiles.Contains(extension)
            || VideoFiles.Contains(extension);
    }
}
