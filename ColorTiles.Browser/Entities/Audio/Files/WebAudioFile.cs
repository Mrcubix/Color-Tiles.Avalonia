using System;
using System.IO;
using ColorTiles.Entities.Audio.Enums;
using ColorTiles.Entities.Audio.Files;
using ColorTiles.Entities.Audio.Files.Readers;
using ColorTiles.Entities.Tools.Managers;

namespace ColorTiles.Browser.Entities.Audio.Files;

using static ColorTiles.Browser.Interop.Audio.AudioFileInterop;

// TODO: Clean this up, this is a mess
public class WebAudioFile : WaveAudioFile
{
    public static long NextID { get; private set; } = 0;

    public long ID { get; set; }

    public WebAudioFile()
    {
        ID = NextID++;

        Buffer = new byte[0];
        Channels = -1;
        SampleRate = -1;
        SampleSize = -1;
        Format = AudioFormat.Raw;
    }

    public WebAudioFile(string path)
    {
        ID = NextID++;

        if (!AudioSetManager.TryLoadAudioAsset(path, out Stream? stream))
            throw new Exception("Failed to load audio file.");

        // create the buffer
        Buffer = new byte[stream!.Length];

        stream!.Read(Buffer, 0, Buffer.Length);

        Channels = -1;
        SampleRate = -1;
        SampleSize = -1;
        Format = AudioFormat.Wave;

        Initialize();

        Volume = 1.0f;
        Pitch = 1.0f;
        DoLoop = false;
    }

    public WebAudioFile(byte[] buffer, int channels, int sampleRate, int sampleSize)
    {
        ID = NextID++;

        Buffer = buffer;
        Channels = channels;
        SampleRate = sampleRate;
        SampleSize = sampleSize;
        Format = AudioFormat.Raw;

        Initialize();

        Volume = 1.0f;
        Pitch = 1.0f;
        DoLoop = false;
    }

    public WebAudioFile(string path, WaveFileReader reader)
    {
        ID = NextID++;

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
        if (Channels != -1 && SampleRate != -1 && SampleSize != -1)
        {
            if(CreateNewFileWithData(ID, Buffer, Channels, SampleRate, SampleSize) == -1)
                throw new Exception("Failed to create new audio file.");
        }
        else
        {
            if(CreateNewFileWithBinary(ID, Buffer) == -1)
                throw new Exception("Failed to create new audio file.");
        }

        if(!SetVariablesForFile(ID, Volume, Pitch, DoLoop))
            throw new Exception("Failed to set variables for audio file.");

        HasLoaded = true;
    }

    public override void Play()
    {
        if (HasLoaded == false)
            return;

        if(!PlayFile(ID))
            throw new Exception("Failed to play audio file.");
    }

    public override void Pause()
    {
        if (HasLoaded == false)
            return;

        if(!PauseFile(ID))
            throw new Exception("Failed to pause audio file.");
    }

    public override void Stop()
    {
        if (HasLoaded == false)
            return;

        if(!StopFile(ID))
            throw new Exception("Failed to stop audio file.");
    }

    protected override void OnVolumeChanged()
    {
        if (HasLoaded == false)
            return;

        if(!SetVolumeForFile(ID, Volume))
            throw new Exception("Failed to set volume for audio file.");
    }

    protected override void OnPitchChanged()
    {
        if (HasLoaded == false)
            return;

        if(!SetPitchForFile(ID, Pitch))
            throw new Exception("Failed to set pitch for audio file.");
    }

    protected override void OnLoopChanged()
    {
        if (HasLoaded == false)
            return;

        if(!SetDoLoopForFile(ID, DoLoop))
            throw new Exception("Failed to set loop for audio file.");
    }

    public override void Dispose()
    {
        if(!DestroyFile(ID))
            throw new Exception("Failed to delete audio file.");
    }
}