using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace ColorTiles.Views.Controls;

public partial class HoverButton : Button
{
    private bool _commandCanExecute = true;
    private bool _isHovered = false;

    protected override Type StyleKeyOverride => typeof(Button);

    public static readonly StyledProperty<ICommand?> HoverCommandProperty = AvaloniaProperty.Register<HoverButton, ICommand?>(nameof(HoverCommand), enableDataValidation: true);
    public static readonly RoutedEvent<RoutedEventArgs> HoverEvent = RoutedEvent.Register<HoverButton, RoutedEventArgs>(nameof(Hover), RoutingStrategies.Bubble);
    public static readonly DirectProperty<HoverButton, bool> IsHoveredProperty = AvaloniaProperty.RegisterDirect<HoverButton, bool>(nameof(IsHovered), b => b.IsHovered);

    public ICommand? HoverCommand
    {
        get
        {
            return GetValue(HoverCommandProperty);
        }
        set
        {
            SetValue(HoverCommandProperty, value);
        }
    }

    public event EventHandler<RoutedEventArgs>? Hover
    {
        add
        {
            AddHandler(HoverEvent, value);
        }
        remove
        {
            RemoveHandler(HoverEvent, value);
        }
    }

    public bool IsHovered
    {
        get => _isHovered;
        private set => SetAndRaise(IsHoveredProperty, ref _isHovered, value);
    }

    static HoverButton()
    {
    }

    public HoverButton() : base()
    {
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);

        if (HoverCommand != null)
        {
            HoverCommand.CanExecuteChanged += CanExecuteChanged;
        }
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);

        if (HoverCommand != null)
        {
            HoverCommand.CanExecuteChanged -= CanExecuteChanged;
        }
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        base.UpdateDataValidation(property, state, error);
        if (property == HoverCommandProperty && state == BindingValueType.BindingError && _commandCanExecute)
        {
            _commandCanExecute = false;
            UpdateIsEffectivelyEnabled();
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == HoverCommandProperty)
        {
            if (((ILogical)this).IsAttachedToLogicalTree)
            {
                var (command, command2) = change.GetOldAndNewValue<ICommand>();

                if (command != null)
                {
                    command.CanExecuteChanged -= CanExecuteChanged;
                }

                if (command2 != null)
                {
                    command2.CanExecuteChanged += CanExecuteChanged;
                }
            }

            CanExecuteChanged(this, EventArgs.Empty);
        }
    }

    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);

        OnHover();
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);

        OnHover();
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);

        IsHovered = false;
    }

    protected virtual void OnHover()
    {
        if (HoverCommand != null && HoverCommand.CanExecute(null) && !IsHovered)
        {
            IsHovered = true;
            HoverCommand.Execute(null);
        }
    }

    private void CanExecuteChanged(object? sender, EventArgs e)
    {
        bool canExecute = Command == null || HoverCommand!.CanExecute(CommandParameter);

        if (canExecute != _commandCanExecute)
        {
            _commandCanExecute = canExecute;
            UpdateIsEffectivelyEnabled();
        }
    }
}