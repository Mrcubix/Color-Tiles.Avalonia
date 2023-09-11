using Avalonia;
using NativePoint = System.Drawing.Point;

namespace ColorTiles.Extensions;

public static class PointExtension
{
    public static NativePoint ToNativePoint(this Point point)
    {
        return new NativePoint((int)point.X, (int)point.Y);
    }

    public static Point Add(this Point point, Point other)
    {
        return new Point(point.X + other.X, point.Y + other.Y);
    }

    public static Point Add(this Point point, NativePoint other)
    {
        return new Point(point.X + other.X, point.Y + other.Y);
    }

    public static Point Add(this Point point, PixelSize other)
    {
        return new Point(point.X + other.Width, point.Y + other.Height);
    }

    public static Point Add(this Point point, Size other)
    {
        return new Point(point.X + other.Width, point.Y + other.Height);
    }

    public static Point Subtract(this Point point, Point other)
    {
        return new Point(point.X - other.X, point.Y - other.Y);
    }

    public static Point Subtract(this Point point, NativePoint other)
    {
        return new Point(point.X - other.X, point.Y - other.Y);
    }

    public static Point Subtract(this Point point, PixelSize other)
    {
        return new Point(point.X - other.Width, point.Y - other.Height);
    }

    public static Point Subtract(this Point point, Size other)
    {
        return new Point(point.X - other.Width, point.Y - other.Height);
    }

    public static Point Multiply(this Point point, Point other)
    {
        return new Point(point.X * other.X, point.Y * other.Y);
    }

    public static Point Multiply(this Point point, NativePoint other)
    {
        return new Point(point.X * other.X, point.Y * other.Y);
    }

    public static Point Multiply(this Point point, PixelSize other)
    {
        return new Point(point.X * other.Width, point.Y * other.Height);
    }

    public static Point Multiply(this Point point, Size other)
    {
        return new Point(point.X * other.Width, point.Y * other.Height);
    }

    public static Point Divide(this Point point, Point other)
    {
        return new Point(point.X / other.X, point.Y / other.Y);
    }

    public static Point Divide(this Point point, NativePoint other)
    {
        return new Point(point.X / other.X, point.Y / other.Y);
    }

    public static Point Divide(this Point point, PixelSize other)
    {
        return new Point(point.X / other.Width, point.Y / other.Height);
    }

    public static Point Divide(this Point point, Size other)
    {
        return new Point(point.X / other.Width, point.Y / other.Height);
    }
}