using Avalonia;

namespace ColorTiles.Extensions
{
    public static class PixelRectExtension
    {
        public static PixelRect FromPixelSizes(PixelSize offset, PixelSize size)
        {
            return new PixelRect(offset.Width, offset.Height, size.Width, size.Height);
        }

        public static PixelRect FromPixelSizes(PixelPoint offset, Size size)
        {
            return new PixelRect(offset.X, offset.Y, (int)size.Width, (int)size.Height);
        }
    }
}