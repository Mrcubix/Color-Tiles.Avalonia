using ColorTiles.Entities.Audio.Files;
using ColorTiles.Entities.Tools;

namespace ColorTiles.Entities.Audio.Sets;

public class GameAudioSet : AudioSet
{
    public GameAudioSet(int id, IAudioFile buttonClickSFX, 
                                IAudioFile buttonHoverSFX, 
                                IAudioFile matchSFX, 
                                IAudioFile penaltySFX, 
                                IAudioFile gameOverSFX) : base(id, buttonClickSFX, buttonHoverSFX, matchSFX, penaltySFX, gameOverSFX)
    {
    }
}