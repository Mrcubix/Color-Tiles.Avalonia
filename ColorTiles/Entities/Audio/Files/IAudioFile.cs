using System;
using ColorTiles.Entities.Audio.Enums;
using ReactiveUI;

namespace ColorTiles.Entities.Audio.Files
{
    public abstract class IAudioFile : ReactiveObject, IDisposable
    {
        /// <summary>
        ///   Pointer to the source generated by OpenAL.
        /// </summary>
        protected int Source { get; set; }
        
        /// <summary>
        ///  Pointer to the buffer generated by OpenAL.
        /// </summary>
        protected int ALBuffer { get; set; }

        protected float _volume;
        protected float _pitch;
        protected bool _doLoop;

        public byte[] Buffer { get; init; }
        public AudioFormat Format { get; init; }
        public int Channels { get; init; }
        public int SampleRate { get; init; }
        public int SampleSize { get; init; }
        public bool HasLoaded { get; protected set; }

        public abstract float Volume { get; set; }
        public abstract float Pitch { get; set; }
        public abstract bool DoLoop { get; set; }

        public IAudioFile()
        {
            Buffer = null!;
        }

        public abstract void Initialize();

        public abstract void Play();

        public abstract void Pause();

        public abstract void Stop();

        public abstract void Dispose();
    }
}