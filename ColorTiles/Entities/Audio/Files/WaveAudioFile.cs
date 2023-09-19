using System;
using ColorTiles.Entities.Audio.Enums;
using ColorTiles.Entities.Audio.Files.Readers;
using OpenTK.Audio.OpenAL;
using ReactiveUI;

namespace ColorTiles.Entities.Audio.Files
{
    public class WaveAudioFile : IAudioFile
    {
        public override float Volume
        {
            get => _volume;
            set
            {
                this.RaiseAndSetIfChanged(ref _volume, value);
                OnVolumeChanged();
            }
        }

        public override float Pitch
        {
            get => _pitch;
            set
            {
                this.RaiseAndSetIfChanged(ref _pitch, value);
                OnPitchChanged();
            }
        }

        public override bool DoLoop
        {
            get => _doLoop;
            set
            {
                this.RaiseAndSetIfChanged(ref _doLoop, value);
                OnLoopChanged();
            }
        }

        public WaveAudioFile()
        {
            Buffer = null!;
            Format = AudioFormat.Wave;

            Initialize();

            Volume = 1.0f;
            Pitch = 1.0f;
            DoLoop = false;
        }

        public WaveAudioFile(byte[] buffer, int channels, int sampleRate, int sampleSize)
        {
            Buffer = buffer;
            Channels = channels;
            SampleRate = sampleRate;
            SampleSize = sampleSize;
            Format = AudioFormat.Wave;

            Initialize();

            Volume = 1.0f;
            Pitch = 1.0f;
            DoLoop = false;
        }

        public WaveAudioFile(string path, WaveFileReader reader)
        {
            IAudioFile file = reader.Read(path);
            
            Buffer = file.Buffer;
            Channels = file.Channels;
            SampleRate = file.SampleRate;
            SampleSize = file.SampleSize;
            Format = AudioFormat.Wave;

            Initialize();

            Volume = 1.0f;
            Pitch = 1.0f;
            DoLoop = false;
        }

        public override void Initialize()
        {
            if (Buffer == null)
                throw new Exception("Buffer is null");

            Source = AL.GenSource();

            if (Source == 0)
            {
                ALError error = AL.GetError();

                string errorString = AL.GetErrorString(error);

                if (string.IsNullOrEmpty(errorString))
                    throw new Exception("Source is 0, no error string provided.");
                else 
                    throw new Exception(errorString);
            }

            ALBuffer = AL.GenBuffer();

            if (ALBuffer == 0)
                throw new Exception(AL.GetErrorString(AL.GetError()));

            var soundSampleFormat = ALFormat.Mono16;

            if (Channels == 1 && SampleSize == 8)
                soundSampleFormat = ALFormat.Mono8;
            else if (Channels == 1 && SampleSize == 16)
                soundSampleFormat = ALFormat.Mono16;
            else if (Channels == 2 && SampleSize == 8)
                soundSampleFormat = ALFormat.Stereo8;
            else if (Channels == 2 && SampleSize == 16)
                soundSampleFormat = ALFormat.Stereo16;

            AL.BufferData(ALBuffer, soundSampleFormat, Buffer, SampleRate);

            if (AL.GetError() != ALError.NoError)
                throw new Exception(AL.GetErrorString(AL.GetError()));

            AL.BindBufferToSource(Source, ALBuffer);

            if (AL.GetError() != ALError.NoError)
                throw new Exception(AL.GetErrorString(AL.GetError()));

            OnInitialized();

            HasLoaded = true;
        }

        public override void Play()
        {
            if (HasLoaded == false)
                throw new Exception("Audio file has not loaded");

            AL.SourcePlay(Source);
        }

        public override void Pause()
        {
            if (HasLoaded == false)
                throw new Exception("Audio file has not loaded");

            AL.SourcePause(Source);
        }

        public override void Stop()
        {
            if (HasLoaded == false)
                throw new Exception("Audio file has not loaded");

            AL.SourceStop(Source);
        }

        private void OnInitialized()
        {
            AL.Source(Source, ALSourcef.Gain, Volume);
            AL.Source(Source, ALSourcef.Pitch, Pitch);
            AL.Source(Source, ALSourceb.Looping, DoLoop);
        }

        private void OnVolumeChanged()
        {
            if (HasLoaded == false)
                return;

            AL.Source(Source, ALSourcef.Gain, Volume);
        }

        private void OnPitchChanged()
        {
            if (HasLoaded == false)
                return;

            AL.Source(Source, ALSourcef.Pitch, Pitch);
        }

        private void OnLoopChanged()
        {
            if (HasLoaded == false)
                return;

            AL.Source(Source, ALSourceb.Looping, DoLoop);
        }

        public override void Dispose()
        {
            AL.DeleteSource(Source);
            AL.DeleteBuffer(ALBuffer);

            Buffer = null!;

            HasLoaded = false;
        }

        // IAudioFile can be casted to WaveAudioFile, but not the other way around
        public static WaveAudioFile FromInterface(IAudioFile file)
        {
            if (file is WaveAudioFile waveFile)
                return waveFile;
            else
                throw new InvalidCastException("IAudioFile is not a WaveAudioFile");
        }
    }
}