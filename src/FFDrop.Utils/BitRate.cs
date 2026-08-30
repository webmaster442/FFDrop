using System.Numerics;

namespace FFDrop.Utils;

internal readonly record struct BitRate :
    IAdditionOperators<BitRate, BitRate, BitRate>,
    ISubtractionOperators<BitRate, BitRate, BitRate>
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

    public static BitRate operator +(BitRate left, BitRate right)
        => new(left._bitsPerSecond + right._bitsPerSecond);

    public static BitRate operator -(BitRate left, BitRate right)
        => new(left._bitsPerSecond - right._bitsPerSecond);
}
