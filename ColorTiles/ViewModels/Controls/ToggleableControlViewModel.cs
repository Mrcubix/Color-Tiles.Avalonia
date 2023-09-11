using ReactiveUI;

namespace ColorTiles.ViewModels.Controls;

public abstract class ToggleableControlViewModel : ViewModelBase
{
    private bool _isEnabled;

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public ToggleableControlViewModel()
    {
        IsEnabled = true;
    }

    public virtual void Toggle()
    {
        IsEnabled = !IsEnabled;
    }

    public virtual void Enable()
    {
        IsEnabled = true;
    }

    public virtual void Disable()
    {
        IsEnabled = false;
    }
}