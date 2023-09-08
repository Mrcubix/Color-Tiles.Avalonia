using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ColorTiles.Entities.Tilesets;
using ColorTiles.Extensions;

namespace ColorTiles.Views.Controls;

public partial class GameBackground : Image
{
    public static readonly StyledProperty<ColorTileSet> TilesetProperty = AvaloniaProperty.Register<GameBackground, ColorTileSet>(nameof(Tileset));

    public static readonly StyledProperty<PixelSize> SizeProperty = AvaloniaProperty.Register<GameBackground, PixelSize>(nameof(Size), new PixelSize(1280, 720));

    public static readonly StyledProperty<PixelSize> OffsetProperty = AvaloniaProperty.Register<GameBackground, PixelSize>(nameof(Offset), new PixelSize(-32, -8));

    public static readonly StyledProperty<Size> ZoomProperty = AvaloniaProperty.Register<GameBackground, Size>(nameof(Zoom), new Size(0.9, 0.9));

    public ColorTileSet Tileset
    {
        get => GetValue(TilesetProperty);
        set => SetValue(TilesetProperty, value);
    }

    public PixelSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public PixelSize Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public Size Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public RenderTargetBitmap? Background => Source as RenderTargetBitmap;
    public PixelRect BackgroundRect => PixelRectExtension.FromPixelSizes(Offset, Size.Divide(Zoom));

    public GameBackground()
    {
        InitializeComponent();

        Stretch = Stretch.Fill;
        StretchDirection = StretchDirection.Both;

        DrawBackground();
    }

    private void DrawBackground()
    {
        PixelRect backgroundRect = BackgroundRect;

        Source = new RenderTargetBitmap(backgroundRect.Size, new Vector(96, 96));

        if (Tileset == null)
            return;

        var textureResolution = Tileset.TextureResolution;
        Rect sourceRect = new(0, 0, textureResolution.Width, textureResolution.Height);

        // This variable is used to alternate between drawing the light and dark background tiles
        bool drawLightTile = false;

        if (Background == null)
            return;

        // Draw images in alternating pattern
        using DrawingContext context = Background.CreateDrawingContext();
        
        for (int x = backgroundRect.X; x < backgroundRect.Width; x += textureResolution.Width)
        {
            for (int y = backgroundRect.Y; y < backgroundRect.Height; y += textureResolution.Height)
            {
                if (drawLightTile)
                    context.DrawImage(Tileset.BackgroundTileLight, sourceRect, new Rect(x, y, textureResolution.Width, textureResolution.Height));
                else
                    context.DrawImage(Tileset.BackgroundTileDark, sourceRect, new Rect(x, y, textureResolution.Width, textureResolution.Height));

                drawLightTile = !drawLightTile;
            }
        }

        context.Dispose();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == TilesetProperty)
        {
            DrawBackground();
        }
    }
}