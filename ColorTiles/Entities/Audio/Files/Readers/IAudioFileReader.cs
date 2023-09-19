using System;

namespace ColorTiles.Entities.Audio.Files.Readers
{
    public interface IAudioFileReader : IDisposable
    {
        public IAudioFile Read(string path);
    }
}