using System;
using ColorTiles.Entities.Audio.Files;
using ReactiveUI;

namespace ColorTiles.Entities.Tools;

public class AudioSet : ReactiveObject, IDisposable
{
    public int ID { get; set; }
    
    public IAudioFile ButtonClickSFX { get; }
    public IAudioFile ButtonHoverSFX { get; }
    public IAudioFile MatchSFX { get; }
    public IAudioFile PenaltySFX { get; }
    public IAudioFile GameOverSFX { get; }

    public AudioSet(int id, IAudioFile buttonClickSFX, IAudioFile buttonHoverSFX, IAudioFile matchSFX, IAudioFile penaltySFX, IAudioFile gameOverSFX)
    {
        ID = id;
        ButtonClickSFX = buttonClickSFX;
        ButtonHoverSFX = buttonHoverSFX;
        MatchSFX = matchSFX;
        PenaltySFX = penaltySFX;
        GameOverSFX = gameOverSFX;
    }

    public void Dispose()
    {
        ButtonClickSFX.Dispose();
        ButtonHoverSFX.Dispose();
        MatchSFX.Dispose();
        PenaltySFX.Dispose();
        //GameOverSFX.Dispose();
    }
}