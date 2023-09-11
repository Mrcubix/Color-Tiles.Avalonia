using System;
using NativePoint = System.Drawing.Point;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using ColorTiles.Entities;
using ColorTiles.Extensions;
using ColorTiles.ViewModels.Controls;
using System.Diagnostics;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;

namespace ColorTiles.Views.Controls;

public partial class GameBoard : Control
{
    /*public static readonly StyledProperty<GameTileSet> TilesetProperty = AvaloniaProperty.Register<GameBoard, GameTileSet>(nameof(Tileset));

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<GameBoard, int>(nameof(Columns), 23);

    public static readonly StyledProperty<int> RowsProperty = AvaloniaProperty.Register<GameBoard, int>(nameof(Rows), 15);

    public static readonly StyledProperty<int> TilesPerColorProperty = AvaloniaProperty.Register<GameBoard, int>(nameof(TilesPerColor), 20);

    public static readonly StyledProperty<PixelSize> OffsetProperty = AvaloniaProperty.Register<GameBackground, PixelSize>(nameof(Offset), new PixelSize(3 * 48 + 16, 32));

    public static readonly StyledProperty<Size> ZoomProperty = AvaloniaProperty.Register<GameBackground, Size>(nameof(Zoom), new Size(0.9, 0.9));

    /// <summary>
    ///   Tileset used to render the game board. <br/>
    ///   Tiles 0 to 8 are used for the game board. <br/>
    /// </summary>
    public GameTileSet Tileset
    {
        get => GetValue(TilesetProperty);
        set => SetValue(TilesetProperty, value);
    }

    /// <summary>
    ///   Numbers of columns in the game board. <br/>
    ///   Used for the array of <see cref="ColorTile"/>s and <br/>
    ///   the final size of the image representing the content of the game board.
    /// </summary>
    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    /// <summary>
    ///   Numbers of rows in the game board. <br/>
    ///   Used for the array of <see cref="ColorTile"/>s and <br/>
    ///   the final size of the image representing the content of the game board.
    /// </summary>
    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    /// <summary>
    ///   Number of tiles per color. <br/>
    /// </summary>
    public int TilesPerColor
    {
        get => GetValue(TilesPerColorProperty);
        set => SetValue(TilesPerColorProperty, value);
    }

    /// <summary>
    ///   Offset of the game board. <br/>
    /// </summary>
    public PixelSize Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    /// <summary>
    ///   Zoom of the game board. <br/>
    /// </summary>
    public Size Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }*/

    private Size _baseSize = new(1280, 720);
    private RenderTargetBitmap _blankTile = new(new PixelSize(48, 48), new Vector(96, 96));

    public event EventHandler<Point>? Clicked;

    public GameBoard()
    {
        DataContext = new GameBoardViewModel();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        //_baseSize = Bounds.Size;
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is GameBoardViewModel viewModel)
        {
            viewModel.BoardCleared -= (_, _) => InvalidateVisual();
            viewModel.BoardGenerated -= (_, _) => InvalidateVisual();
            viewModel.MatchesFound -= (_, _) => InvalidateVisual();
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is GameBoardViewModel viewModel)
        {
            viewModel.BoardCleared += (_, _) => InvalidateVisual();
            viewModel.BoardGenerated += (_, _) => InvalidateVisual();
            viewModel.MatchesFound += (_, _) => InvalidateVisual();
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (DataContext is not GameBoardViewModel viewmodel)
            return;

        if (viewmodel.Tileset == null)
            return;

        if (viewmodel.Board == null || viewmodel.Board.Count == 0)
            return;

        if (_baseSize == default)
            _baseSize = Bounds.Size;

        var textureResolution = viewmodel.Tileset.TextureResolution;

        var sourceRect = new Rect(0, 0, textureResolution.Width, textureResolution.Height);

        Size scale = Bounds.Size.Divide(_baseSize);

        // the offset is affected by the zoom and the current window size
        Size CorrectedOffset = viewmodel.Zoom.Multiply(viewmodel.Offset).Multiply(scale);
        // the size of the tile is affected by the zoom and the current window size
        Size destSize = viewmodel.Zoom.Multiply(textureResolution).Multiply(scale);
        viewmodel.ScaledTileSize = destSize;

        // draw tiles starting from the offset, final image should be Rows * Columns pixels
        for (int y = 0; y < viewmodel.Rows; y++)
        {
            for (int x = 0; x < viewmodel.Columns; x++)
            {
                var tile = viewmodel.GetTileAt(x, y);

                IImage tileImage = tile == null ? _blankTile : viewmodel.Tileset.Tiles[tile.AtlasIndex];

                // The previous calculations affect the calculation of the position of the tile
                Point pos = new(x * destSize.Width + CorrectedOffset.Width, y * destSize.Height + CorrectedOffset.Height);

                context.DrawImage(tileImage, sourceRect, new Rect(pos, destSize));
            }
        }
    }

    /*protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == TilesetProperty)
        {
            if (DataContext is GameBoardViewModel viewModel)
            {
                viewModel.Tileset = Tileset;
            }
        }

        if (change.Property == ColumnsProperty)
        {
            if (DataContext is GameBoardViewModel viewModel)
            {
                viewModel.Columns = Columns;
            }
        }

        if (change.Property == RowsProperty)
        {
            if (DataContext is GameBoardViewModel viewModel)
            {
                viewModel.Rows = Rows;
            }
        }

        if (change.Property == TilesPerColorProperty)
        {
            if (DataContext is GameBoardViewModel viewModel)
            {
                viewModel.TilesPerColor = TilesPerColor;
            }
        }
    }*/

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            OnClicked(e.GetPosition(this));
        }
    }

    public void OnClicked(Point relativePosition)
    {
        if (_baseSize == default)
            return;

        if (DataContext is GameBoardViewModel viewmodel)
        {
            Size scale = Bounds.Size.Divide(_baseSize);

            PixelSize correctedOffset = viewmodel.Offset.Multiply(viewmodel.Zoom).Multiply(scale);
            Point correctedPos = relativePosition.Subtract(correctedOffset);

            Clicked?.Invoke(this, correctedPos);

            viewmodel.OnBoardClicked(correctedPos.ToNativePoint());
        }
    }
}