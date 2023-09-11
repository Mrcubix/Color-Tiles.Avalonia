using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ColorTiles.ViewModels.Controls.HUD;

namespace ColorTiles.Views.Controls.HUD;

public class TimerBar : Image
{
    public RenderTargetBitmap Rectangle { get; set; }

    public TimerBar()
    {
        Rectangle = new RenderTargetBitmap(new PixelSize(720, 30));
        Source = Rectangle;
    }

    protected override void OnDataContextBeginUpdate()
    {
        base.OnDataContextBeginUpdate();

        if (DataContext is TimerBarViewModel viewModel)
        {
            viewModel.IntervalElapsed -= OnIntervalElapsed;
            viewModel.PenaltyInflicted -= OnPenaltyInflicted;
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is TimerBarViewModel viewModel)
        {
            viewModel.IntervalElapsed += OnIntervalElapsed;
            viewModel.PenaltyInflicted += OnPenaltyInflicted;
        }
    }

    private void OnIntervalElapsed(object? sender, EventArgs e)
    {
        ReDrawTimerBar();
    }

    private void OnPenaltyInflicted(object? sender, EventArgs e)
    {
        ReDrawTimerBar();
    }

    private void ReDrawTimerBar()
    {
        using var context = Rectangle.CreateDrawingContext();

        if (DataContext is not TimerBarViewModel viewModel)
            return;

        if (viewModel.RemainingTime <= 0)
            return;

        // width is equal to current time / initial time * the 
        double width = (double)viewModel.RemainingTime / (double)viewModel.InitialTime * 720;

        context.FillRectangle(Brushes.Red, new Rect(0, 0, width, 30));

        InvalidateVisual();
    }
}