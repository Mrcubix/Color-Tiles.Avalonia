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

    public event EventHandler<Point>? Clicked;

    public GameBoard()
    {
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