using Avalonia;

namespace ColorTiles.Extensions;

public static class PixelSizeExtension
{
    public static PixelSize Multiply(this PixelSize size, PixelSize multiplier)
    {
        return new PixelSize(size.Width * multiplier.Width, size.Height * multiplier.Height);
    }

    public static PixelSize Multiply(this PixelSize size, Size multiplier)
    {
        return new PixelSize((int)(size.Width * multiplier.Width), (int)(size.Height * multiplier.Height));
    }

    public static PixelSize Divide(this PixelSize size, PixelSize divider)
    {
        return new PixelSize(size.Width / divider.Width, size.Height / divider.Height);
    }

    public static PixelSize Divide(this PixelSize size, Size divider)
    {
        return new PixelSize((int)(size.Width / divider.Width), (int)(size.Height / divider.Height));
    }
}