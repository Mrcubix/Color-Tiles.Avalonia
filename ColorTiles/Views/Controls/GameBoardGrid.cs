using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using ColorTiles.Entities;
using ColorTiles.Extensions;
using ColorTiles.ViewModels.Controls;

namespace ColorTiles.Views.Controls;

public partial class GameBoardGrid : Grid
{
    public event EventHandler<Point>? Clicked;

    public GameBoardGrid()
    {
        //InitializeComponent();
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is GameBoardViewModel viewModel)
        {
            viewModel.BoardCleared -= OnBoardCleared;
            viewModel.BoardGenerated -= OnBoardGenerated;
            viewModel.TilesRemovalRequested -= OnTilesRemovalRequested;

            viewModel.SettingsChanged -= OnSettingsChanged;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is GameBoardViewModel viewModel)
        {
            viewModel.BoardCleared += OnBoardCleared;
            viewModel.BoardGenerated += OnBoardGenerated;
            viewModel.TilesRemovalRequested += OnTilesRemovalRequested;

            viewModel.SettingsChanged += OnSettingsChanged;
        }
    }

    private void ResetDefinitions()
    {
        if (DataContext is not GameBoardViewModel viewmodel)
            return;

        if (viewmodel.Tileset == null)
            return;

        Children.Clear();

        // Clear all definitions
        ColumnDefinitions.Clear();

        ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));

        // we need to add as many definitions as there are columns
        for (int i = 0; i < viewmodel.Columns; i++)
        {
            ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
        }

        ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));

        // Clear all definitions
        RowDefinitions.Clear();

        RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));

        // we need to add as many definitions as there are rows
        for (int i = 0; i < viewmodel.Rows; i++)
        {
            RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
        }

        RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
    }

    private void BuildGridElements()
    {
        if (DataContext is not GameBoardViewModel viewmodel)
            return;

        if (viewmodel.Tileset == null)
            return;

        // Clear all children
        Children.Clear();

        // Add the tiles
        for(int x = 0; x < viewmodel.Columns; x++)
        {
            for(int y = 0; y < viewmodel.Rows; y++)
            {
                var tile = viewmodel.GetTileAt(x, y);

                if (tile == null)
                    continue;

                var image = new Image
                {
                    Source = viewmodel.Tileset.Tiles[tile.AtlasIndex],
                    Stretch = Stretch.Fill
                };

                SetColumn(image, x + 1);
                SetRow(image, y + 1);

                Children.Add(image);
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
        if (DataContext is GameBoardViewModel viewmodel)
        {
            Point correctedPos = relativePosition.Subtract(viewmodel.Offset);
            Clicked?.Invoke(this, correctedPos);

            viewmodel.OnBoardClicked(correctedPos.ToNativePoint());
        }
    }

    private void OnSettingsChanged(object? sender, EventArgs e)
    {
        if (DataContext is not GameBoardViewModel viewmodel)
            return;

        if (viewmodel.Tileset == null)
            return;

        ResetDefinitions();
        BuildGridElements();
    }

    private void OnBoardCleared(object? sender, EventArgs e)
    {
        ResetDefinitions();
    }

    private void OnBoardGenerated(object? sender, EventArgs e)
    {
        BuildGridElements();
    }

    private void OnTilesRemovalRequested(object? sender, ColorTile?[] tiles)
    {
        if (DataContext is not GameBoardViewModel viewmodel)
            return;

        if (viewmodel.Tileset == null)
            return;

        foreach (var tile in tiles)
        {
            if (tile == null)
                continue;

            // we need to remove the tile from the grid
            // find the image with the right column and row using the tile's psoition property
            var imageIndex = tile.Position.Y * viewmodel.Columns + tile.Position.X;

            Children.Remove(Children[imageIndex]);
        }
    }
}