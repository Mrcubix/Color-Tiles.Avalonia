using Avalonia;

namespace ColorTiles.Extensions;

public static class SizeExtension
{
    public static Size Multiply(this Size size, Size multiplier)
    {
        return new Size(size.Width * multiplier.Width, size.Height * multiplier.Height);
    }

    public static Size Multiply(this Size size, PixelSize multiplier)
    {
        return new Size(size.Width * multiplier.Width, size.Height * multiplier.Height);
    }

    public static Size Divide(this Size size, Size divider)
    {
        return new Size(size.Width / divider.Width, size.Height / divider.Height);
    }

    public static Size Divide(this Size size, PixelSize divider)
    {
        return new Size(size.Width / divider.Width, size.Height / divider.Height);
    }
}