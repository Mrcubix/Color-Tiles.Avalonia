using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using ColorTiles.Extensions;
using ColorTiles.ViewModels.Controls;
using Avalonia.Media.Imaging;

namespace ColorTiles.Views.Controls;

public partial class GameBoard : Control
{
    private Size _baseSize = new(1280, 720);
    private RenderTargetBitmap _blankTile = new(new PixelSize(48, 48), new Vector(96, 96));
    private Size _correctedOffset = default;
    private Size currentScale = new(1, 1);
    private Size currentTileSize = new(48, 48);
    private Rect sourceTileRect = new(0, 0, 48, 48);    

    
    /// <summary>
    ///  Corrected Offset of the game board. <br/>
    ///  The current window scale is taken into account.
    /// </summary>
    private Size CorrectedOffset
    {
        get => _correctedOffset;
        set
        {
            _correctedOffset = value;
            OffsetChanged?.Invoke(this, value);
        }
    }

    public event EventHandler<Size> OffsetChanged;

    public GameBoard()
    {
        OffsetChanged = null!;
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is GameBoardViewModel viewModel)
        {
            viewModel.BoardCleared -= (_, _) => InvalidateVisual();
            viewModel.BoardGenerated -= (_, _) => InvalidateVisual();
            viewModel.MatchesFound -= (_, _) => InvalidateVisual();

            viewModel.OffsetChanged -= OnOffsetChanged;
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

            viewModel.OffsetChanged += OnOffsetChanged;

            OnOffsetChanged(this, viewModel.Offset);
        }
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        currentScale = Bounds.Size.Divide(_baseSize);

        if (DataContext is GameBoardViewModel viewmodel)
        {
            OnOffsetChanged(this, viewmodel.Offset);

            if (viewmodel.Tileset == null)
                return;

            var textureResolution = viewmodel.Tileset.TextureResolution;

            currentTileSize = viewmodel.Zoom.Multiply(textureResolution).Multiply(currentScale);
            sourceTileRect = new Rect(0, 0, textureResolution.Width, textureResolution.Height);
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

        // draw tiles starting from the offset, final image should be Rows * Columns pixels
        for (int y = 0; y < viewmodel.Rows; y++)
        {
            for (int x = 0; x < viewmodel.Columns; x++)
            {
                var tile = viewmodel.GetTileAt(x, y);

                IImage tileImage = tile == null ? _blankTile : viewmodel.Tileset.Tiles[tile.AtlasIndex];

                // The previous calculations affect the calculation of the position of the tile
                Point pos = new(x * currentTileSize.Width + CorrectedOffset.Width, y * currentTileSize.Height + CorrectedOffset.Height);

                context.DrawImage(tileImage, sourceTileRect, new Rect(pos, currentTileSize));
            }
        }
    }

    #region In-Control events

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
            Point correctedPosition = relativePosition.Subtract(CorrectedOffset);

            // Converting previously calculated position to tile position
            Point position = new((int)(correctedPosition.X / currentTileSize.Width),
                                 (int)(correctedPosition.Y / currentTileSize.Height));

            viewmodel.OnBoardClicked(position.ToNativePoint());
        }
    }

    #endregion

    #region External Events

    private void OnOffsetChanged(object? sender, PixelSize offset)
    {
        if (DataContext is GameBoardViewModel viewmodel)
        {
            // the offset is affected by the zoom and the current window size
            CorrectedOffset = viewmodel.Zoom.Multiply(offset).Multiply(currentScale);
            OffsetChanged?.Invoke(this, CorrectedOffset);
        }
    }

    #endregion
}