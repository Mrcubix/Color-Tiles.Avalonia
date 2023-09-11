using System;
using Avalonia.Threading;
using ReactiveUI;

namespace ColorTiles.ViewModels.Controls.HUD;

public class TimerBarViewModel : ViewModelBase
{
    private long _initialTime;
    private long _remainingTime;

    //private Stopwatch _stopwatch;
    DispatcherTimer _stopwatch;

    public event EventHandler? IntervalElapsed;
    public event EventHandler? TimeExpired;
    public event EventHandler? PenaltyInflicted;

    public long InitialTime
    {
        get => _initialTime;
        set => this.RaiseAndSetIfChanged(ref _initialTime, value);
    }

    public long RemainingTime
    {
        get => _remainingTime;
        set => this.RaiseAndSetIfChanged(ref _remainingTime, value);
    }

    public TimerBarViewModel(long initialTime)
    {
        InitialTime = initialTime;
        RemainingTime = InitialTime;

        //_stopwatch = new Stopwatch();
        _stopwatch = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(20)
        };

        _stopwatch.Tick += OnStopwatchTick;
    }

    public TimerBarViewModel() : this(120000) 
    { 
    }

    public bool StartTimer()
    {
        if (_stopwatch.IsEnabled)
            return false;

        _stopwatch.Start();
        return _stopwatch.IsEnabled;
    }

    public bool StopTimer()
    {
        if (!_stopwatch.IsEnabled)
            return false;

        _stopwatch.Stop();
        return _stopwatch.IsEnabled;
    }

    public void ResetTimer()
    {
        _stopwatch.Stop();
        RemainingTime = InitialTime;
    }

    public void RestartTimer()
    {
        ResetTimer();
        StartTimer();
    }

    private void OnStopwatchTick(object? sender, EventArgs e)
    {
        RemainingTime -= _stopwatch.Interval.Milliseconds;

        if (RemainingTime <= 0)
        {
            _stopwatch.Stop();
            RemainingTime = 0;

            TimeExpired?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            IntervalElapsed?.Invoke(this, EventArgs.Empty);
        }
    }

    public void InflictPenalty(long penalty)
    {
        RemainingTime -= penalty;

        PenaltyInflicted?.Invoke(this, EventArgs.Empty);
    }
}