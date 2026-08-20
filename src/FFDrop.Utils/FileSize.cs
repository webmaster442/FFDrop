namespace FFDrop.Utils;

internal readonly struct FileSize
{
    private readonly long _bytes;

    public FileSize(long bytes)
    {
        _bytes = bytes;
    }

    public override string ToString()
    {
        if (_bytes < 1024)
            return $"{_bytes} B";
        else if (_bytes < 1024 * 1024)
            return $"{_bytes / 1024.0:F2} KB";
        else if (_bytes < 1024 * 1024 * 1024)
            return $"{_bytes / (1024.0 * 1024.0):F2} MB";
        else
            return $"{_bytes / (1024.0 * 1024.0 * 1024.0):F2} GB";
    }

    public static FileSize FromBytes(long bytes) 
        => new(bytes);

    public static FileSize FromKilobytes(double kilobytes)
        => new((long)(kilobytes * 1024));

    public static FileSize FromMegabytes(double megabytes)
        => new((long)(megabytes * 1024 * 1024));
}
