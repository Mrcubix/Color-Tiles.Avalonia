using System.Collections.Generic;
using Avalonia.Media;
using ColorTiles.Tools;

namespace ColorTiles.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    public TileSetManager TileSetManager { get; set; }

    public MainViewModel()
    {
        TileSetManager = new TileSetManager();
    }

    public void LoadAssets(List<IImage> assets)
    {

    }
}
