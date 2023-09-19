using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Platform;
using ColorTiles.Entities.Audio.Files.Readers;
using OpenTK.Audio.OpenAL;

namespace ColorTiles.Entities.Tools.Managers;

public class AudioSetManager : IManager<AudioSet>
{
    private const string ASSETS_LOCATION = "avares://ColorTiles/Assets";

    // temporary public until Audio sets are implemented
    public const string DefaultButtonClickSFXPath = "SFX_Button_Click_Trimmed.wav";
    public const string DefaultButtonHoverSFXPath = "SFX_Button_Hover_Trimmed.wav";
    public const string DefaultMatchSFXPath = "SFX_Match_Trimmed.wav";
    public const string DefaultPenaltySFXPath = "SFX_Penalty_Trimmed.wav";
    public const string DefaultGameOverSFXPath = "SFX_Game_Over_Trimmed.wav";

    private static ALContext Context { get; set; }
    private static ALDevice Device { get; set; }

    // To be Implemented
    public List<AudioSet> AudioSets { get; private set; }

    public AudioSetManager()
    {
        AudioSets = new List<AudioSet>();
    }

    public override void Add(AudioSet tileSet)
    {
        AudioSets.Add(tileSet);
    }

    public override void Remove(AudioSet tileSet)
    {
        AudioSets.Remove(tileSet);
    }

    public override void Remove(int index)
    {
        AudioSets.RemoveAt(index);
    }

    public override AudioSet Get(int id)
    {
        return AudioSets[id];
    }

    #region Load

    public override AudioSet LoadDefault()
    {
        if (!TryLoadAudioAsset(DefaultButtonClickSFXPath, out Stream? buttonClickSFXStream))
        {
            throw new Exception("Failed to load button click SFX.");
        }

        if (!TryLoadAudioAsset(DefaultButtonHoverSFXPath, out Stream? buttonHoverSFXStream))
        {
            throw new Exception("Failed to load button hover SFX.");
        }

        if (!TryLoadAudioAsset(DefaultMatchSFXPath, out Stream? matchSFXStream))
        {
            throw new Exception("Failed to load match found SFX.");
        }

        if (!TryLoadAudioAsset(DefaultPenaltySFXPath, out Stream? penaltySFXStream))
        {
            throw new Exception("Failed to load penalty SFX.");
        }

        /*if (!TryLoadAudioAsset(DefaultGameOverSFXPath, out Stream? gameOverSFXStream))
        {
            throw new Exception("Failed to load game over SFX.");
        }*/

        // Initialize the audio files
        var reader = new WaveFileReader();

        var newSet = new AudioSet(0, reader.Read(buttonClickSFXStream!)!, 
                                     reader.Read(buttonHoverSFXStream!)!, 
                                     reader.Read(matchSFXStream!)!,
                                     reader.Read(penaltySFXStream!)!, 
                                     null!);

        // Add the new set to the list
        Add(newSet);

        return newSet;
    }

    private bool TryLoadAudioAsset(string path, out Stream? output)
    {
        output = null;

        try
        {
            output = AssetLoader.Open(new Uri($"{ASSETS_LOCATION}/Audio/{path}"));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion

    #region Audio Initialization

    public void InitializeAudio()
    {
        var Device = ALC.OpenDevice(null);

        if (Device.Handle == IntPtr.Zero)
            throw new Exception("Failed to open audio device.");

        var attributes = new ALContextAttributes();

        var Context = ALC.CreateContext(Device, attributes);

        if (Context.Handle == IntPtr.Zero)
            throw new Exception("Failed to create audio context.");

        if (!ALC.MakeContextCurrent(Context))
            throw new Exception("Failed to make audio context current.");
    }

    #endregion

    #region Disposal

    public override void Dispose()
    {
        // Dispose the audio files
        foreach (var tileSet in AudioSets)
        {
            tileSet.Dispose();
        }

        // Dispose the audio context
        if (Context.Handle != IntPtr.Zero)
        {
            ALC.MakeContextCurrent(ALContext.Null);
            ALC.DestroyContext(Context);
        }

        // Dispose the audio device
        if (Device.Handle != IntPtr.Zero)
            ALC.CloseDevice(Device);
    }

    #endregion
}