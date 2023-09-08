using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using ColorTiles.Entities.Tilesets;
using ColorTiles.ViewModels.Controls;

namespace ColorTiles.Views.Controls;

public partial class GameBoard : UserControl
{
    public static readonly StyledProperty<ColorTileSet> TilesetProperty = AvaloniaProperty.Register<GameBoard, ColorTileSet>(nameof(Tileset));

    public static readonly StyledProperty<int> RowsProperty = AvaloniaProperty.Register<GameBoard, int>(nameof(Rows), 0);

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<GameBoard, int>(nameof(Columns), 0);

    public ColorTileSet Tileset
    {
        get => GetValue(TilesetProperty);
        set => SetValue(TilesetProperty, value);
    }

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public event EventHandler<Point>? Clicked;

    public GameBoard()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is GameBoardViewModel viewModel)
        {
            viewModel.Tileset = Tileset;
            viewModel.Rows = Rows;
            viewModel.Columns = Columns;
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
        Clicked?.Invoke(this, relativePosition);

        if (DataContext is GameBoardViewModel viewModel)
        {
            viewModel.OnBoardClicked(relativePosition);
        }
    }
}