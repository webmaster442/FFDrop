namespace FFDrop.Utils;

internal readonly struct BitRate
{
    private readonly long _bitsPerSecond;

    public BitRate(long bitsPerSecond)
    {
        _bitsPerSecond = bitsPerSecond;
    }

    public override string ToString()
    {
        if (_bitsPerSecond < 1000)
            return $"{_bitsPerSecond} bps";
        else if (_bitsPerSecond < 1000 * 1000)
            return $"{_bitsPerSecond / 1000.0:F2} Kbps";
        else if (_bitsPerSecond < 1000 * 1000 * 1000)
            return $"{_bitsPerSecond / (1000.0 * 1000.0):F2} Mbps";
        else
            return $"{_bitsPerSecond / (1000.0 * 1000.0 * 1000.0):F2} Gbps";
    }

    public static BitRate FromBps(long bitsPerSecond)
        => new(bitsPerSecond);
}
