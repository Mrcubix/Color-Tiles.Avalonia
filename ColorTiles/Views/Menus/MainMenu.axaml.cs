using System;
using ColorTiles.ViewModels.Menus;
using ColorTiles.Views.Controls;

namespace ColorTiles.Views.Menus
{
    public partial class MainMenu : ToggleableControl
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

        public void OnPlayButtonClicked(object? sender, EventArgs e)
        {
            Hide();

            PlayButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}