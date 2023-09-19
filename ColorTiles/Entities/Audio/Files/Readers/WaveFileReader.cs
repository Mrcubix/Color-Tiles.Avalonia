using System;
using System.IO;

namespace ColorTiles.Entities.Audio.Files.Readers;

public class WaveFileReader : IAudioFileReader
{
    private BinaryReader reader;
    private string lastPath;
    private IAudioFile cachedFile;

    public WaveFileReader()
    {
        reader = null!;
        lastPath = string.Empty;
        cachedFile = null!;
    }

    public IAudioFile Read(string path)
    {
        // check if file is a .wav file
        if (path.EndsWith(".wav") == false)
            throw new ArgumentException("File is not a .wav file", nameof(path));

        // check if file exists
        if (File.Exists(path) == false)
            throw new FileNotFoundException("File does not exist", path);

        Stream stream = null!;

        // Open file stream
        if (path == lastPath)
            stream = new MemoryStream(cachedFile.Buffer);
        else
            stream = File.OpenRead(path);

        IAudioFile file = Read(stream, false);

        if (path != lastPath)
        {
            cachedFile = file;
            lastPath = path;
        }

        return file;
    }

    public IAudioFile Read(Stream stream, bool doCache = true)
    {
        reader = new BinaryReader(stream);

        string signature = new(reader.ReadChars(4));

        if (signature != "RIFF")
            throw new InvalidDataException("File is not a wave file");

        reader.ReadInt32(); // file length minus first 8 bytes of RIFF description, we don't need it

        string format = new(reader.ReadChars(4));

        if (format != "WAVE")
            throw new InvalidDataException("File is not a wave file");

        string formatSignature = new(reader.ReadChars(4));

        if (formatSignature != "fmt ")
            throw new InvalidDataException("Only wave files with fmt format signature are supported");

        reader.ReadInt32(); // format length, we don't need it
        reader.ReadInt16(); // format type, we don't need it
        int channels = reader.ReadInt16();
        int sampleRate = reader.ReadInt32();
        reader.ReadInt32(); // byte rate, we don't need it
        reader.ReadInt16(); // block align, we don't need it
        int bitsPerSample = reader.ReadInt16();

        string dataStart = new(reader.ReadChars(4));

        while (dataStart != "data")
        {
            var size = reader.ReadInt32();
            reader.ReadBytes(size);

            dataStart = new string(reader.ReadChars(4));
        }

        int data_chunk_size = reader.ReadInt32();

        byte[] buffer = reader.ReadBytes(data_chunk_size);

        if (stream is MemoryStream)
        {
            stream.Close();
            stream.Dispose();
        }

        var file = new WaveAudioFile(buffer, channels, sampleRate, bitsPerSample);

        if (doCache)
        {
            cachedFile = file;
            lastPath = string.Empty;
        }

        return file;
    }

    public void Dispose()
    {
        reader.Close();
        reader.Dispose();
    }
}