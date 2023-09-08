using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using ColorTiles.ViewModels.Menus;

namespace ColorTiles.Views.Menus
{
    public partial class MainMenu : UserControl
    {
        public event EventHandler? PlayButtonClicked;

        public MainMenu()
        {
            InitializeComponent();
        }

        protected override void OnDataContextBeginUpdate()
        {
            base.OnDataContextBeginUpdate();

            if (DataContext is MainMenuViewModel viewModel)
            {
                viewModel.PlayButtonClicked -= OnPlayButtonClicked;
            }
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);

            if (DataContext is MainMenuViewModel viewModel)
            {
                viewModel.PlayButtonClicked += OnPlayButtonClicked;
            }
        }

        public void Show()
        {
            IsVisible = true;
            IsEnabled = true;
        }

        public void Hide()
        {
            IsVisible = false;
            IsEnabled = false;
        }

        public void OnPlayButtonClicked(object? sender, EventArgs e)
        {
            Hide();

            PlayButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}