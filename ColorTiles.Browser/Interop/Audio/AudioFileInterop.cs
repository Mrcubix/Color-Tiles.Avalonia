using System.Runtime.InteropServices.JavaScript;

namespace ColorTiles.Browser.Interop.Audio;

using static System.Runtime.InteropServices.JavaScript.JSType;

public static partial class AudioFileInterop
{
    /// <summary>
    /// Create a new file using the next available id
    /// </summary>
    /// <returns></returns>
    /*[JSImport("createNewFile", "./AudioFile.js")]
    internal static partial int CreateNewFile();*/

    /// <summary>
    /// Create a new file with the specified id
    /// </summary>
    /// <param name="id">The id of the new file</param>
    /// <returns>The id of the new file or -1 if the id is already in use</returns>
    [JSImport("createNewFileWithID", "AudioFile.js")]
    internal static partial int CreateNewFileWithID([JSMarshalAs<Number>] long id);

    /// <summary>
    /// Create a new file with the specified id & parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="buffer"></param>
    /// <param name="channels"></param>
    /// <param name="sampleRate"></param>
    /// <param name="sampleSize"></param>
    /// <returns>The id of the new file or -1 if the id is already in use</returns>
    [JSImport("createNewFileWithData", "AudioFile.js")]
    internal static partial int CreateNewFileWithData([JSMarshalAs<Number>] long id, [JSMarshalAs<Array<Number>>] byte[] buffer, [JSMarshalAs<Number>] int channels, [JSMarshalAs<Number>] int sampleRate, [JSMarshalAs<Number>] int sampleSize);

    /// <summary>
    /// Create a new file with the specified id & binary data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="binary"></param>
    /// <returns>The id of the new file or -1 if the id is already in use</returns>
    [JSImport("createNewFileWithBinary", "AudioFile.js")]
    internal static partial int CreateNewFileWithBinary([JSMarshalAs<Number>] long id, [JSMarshalAs<Array<Number>>] byte[] binary);

    /// <summary>
    /// Set the volume, pitch, and doLoop variables for the file with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="volume"></param>
    /// <param name="pitch"></param>
    /// <param name="doLoop"></param>
    /// <returns>True if the variables were set, false if the file was not found</returns>
    [JSImport("setVariablesForFile", "AudioFile.js")]
    internal static partial bool SetVariablesForFile([JSMarshalAs<Number>] long id, [JSMarshalAs<Number>] float volume, [JSMarshalAs<Number>] float pitch, [JSMarshalAs<Boolean>] bool doLoop);

    /// <summary>
    /// Set the volume for the file with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="volume"></param>
    /// <returns>True if the volume was set, false if the file was not found</returns>
    [JSImport("setVolumeForFile", "AudioFile.js")]
    internal static partial bool SetVolumeForFile([JSMarshalAs<Number>] long id, [JSMarshalAs<Number>] float volume);

    /// <summary>
    /// Set the pitch for the file with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pitch"></param>
    /// <returns>True if the pitch was set, false if the file was not found</returns>
    [JSImport("setPitchForFile", "AudioFile.js")]
    internal static partial bool SetPitchForFile([JSMarshalAs<Number>] long id, [JSMarshalAs<Number>] float pitch);

    /// <summary>
    /// Set whether the file with the specified id should loop
    /// </summary>
    /// <param name="id"></param>
    /// <param name="doLoop"></param>
    /// <returns>True if the doLoop was set, false if the file was not found</returns>
    [JSImport("setDoLoopForFile", "AudioFile.js")]
    internal static partial bool SetDoLoopForFile([JSMarshalAs<Number>] long id, [JSMarshalAs<Boolean>] bool doLoop);

    /// <summary>
    /// Play the file with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if the file was played, false if the file was not found</returns>
    [JSImport("playFile", "AudioFile.js")]
    internal static partial bool PlayFile([JSMarshalAs<Number>] long id);

    /// <summary>
    /// Pause the file with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if the file was paused, false if the file was not found</returns>
    [JSImport("pauseFile", "AudioFile.js")]
    internal static partial bool PauseFile([JSMarshalAs<Number>] long id);

    /// <summary>
    /// Stop the file with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if the file was stopped, false if the file was not found</returns>
    [JSImport("stopFile", "AudioFile.js")]
    internal static partial bool StopFile([JSMarshalAs<Number>] long id);

    /// <summary>
    /// Destroy the file with the specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if the file was destroyed, false if the file was not found</returns>
    [JSImport("destroyFile", "AudioFile.js")]
    internal static partial bool DestroyFile([JSMarshalAs<Number>] long id);
}