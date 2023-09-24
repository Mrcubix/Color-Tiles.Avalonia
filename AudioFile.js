// ------------------ Variables ------------------ //

let files = [];
let fileIds = [];

// ------------------ Exports ------------------ //

/**
 * Create a new file using the next available id
 * @returns {number} The id of the new file
 */
export function createNewFile()
{
    var file = new AudioFile(files.length);
    files.push(file);
    fileIds.push(file.id);

    return file.id;
}

/**
 * Create a new file with the specified id
 * @param {Number} id 
 * @returns {Number} The id of the new file or -1 if the id is already in use
 */
export function createNewFileWithID(id)
{
    if (fileIdExists(id))
        return -1;

    var file = new AudioFile(id);
    files.push(file);
    fileIds.push(id);

    return id;
}

/**
 * Create a new file with the specified id and data
 * @param {Number} id 
 * @param {Array} buffer 
 * @param {Number} channels 
 * @param {Number} sampleRate 
 * @param {Number} sampleSize 
 * @returns {Number} The id of the new file or -1 if the id is already in use
 */
export function createNewFileWithData(id, buffer, channels, sampleRate, sampleSize)
{
    if (fileIdExists(id))
        return -1;

    var file = new AudioFile(id, buffer, channels, sampleRate, sampleSize);
    files.push(file);
    fileIds.push(id);

    return id;
}

/**
 * Create a new file with the specified id and binary data
 * @param {Number} id 
 * @param {Array} binary 
 * @returns {Number} The id of the new file or -1 if the id is already in use
 */
export function createNewFileWithBinary(id, binary)
{
    if (fileIdExists(id))
        return -1;

    var file = new AudioFile(id, null, null, null, null);
    file.loadFromBinary(binary);
    files.push(file);
    fileIds.push(id);

    return id;
}

/**
 * Set the volume, pitch, and doLoop variables for the file with the specified id
 * @param {Number} id 
 * @param {Number} volume 
 * @param {Number} pitch 
 * @param {Boolean} doLoop 
 * @returns {Boolean} True if the variables were set, false if the file was not found
 */
export function setVariablesForFile(id, volume, pitch, doLoop)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.volume = volume;
    file.pitch = pitch;
    file.doLoop = doLoop;

    return true;
}

/**
 * Set the volume for the file with the specified id
 * @param {Number} id 
 * @param {Number} volume 
 * @returns {Boolean} True if the volume was set, false if the file was not found
 */
export function setVolumeForFile(id, volume)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.volume = volume;

    return true;
}

/**
 * Set the pitch for the file with the specified id
 * @param {Number} id 
 * @param {Number} pitch 
 * @returns {Boolean} True if the pitch was set, false if the file was not found
 */
export function setPitchForFile(id, pitch)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.pitch = pitch;

    return true;
}

/**
 * Set whether the file with the specified id should loop
 * @param {Number} id 
 * @param {Boolean} doLoop 
 * @returns {Boolean} True if the doLoop was set, false if the file was not found
 */
export function setDoLoopForFile(id, doLoop)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.doLoop = doLoop;

    return true;
}

/**
 * Play the file with the specified id
 * @param {Number} id 
 * @returns {Boolean} True if the file was played, false if the file was not found
 */
export function playFile(id)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.play();

    return true;
}

/**
 * Pause the file with the specified id
 * @param {Number} id 
 * @returns {Boolean} True if the file was paused, false if the file was not found
 */
export function pauseFile(id)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.pause();

    return true;
}

/**
 * Stop the file with the specified id
 * @param {Number} id 
 * @returns {Boolean} True if the file was stopped, false if the file was not found
 */
export function stopFile(id)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.stop();

    return true;
}

/**
 * Destroy the file with the specified id
 * @param {Number} id 
 * @returns {Boolean} True if the file was destroyed, false if the file was not found
 */
export function destroyFile(id)
{
    let file = getFile(id);

    if (file == null)
        return false;

    file.stop();

    files.splice(fileIds.indexOf(id), 1);
    fileIds.splice(fileIds.indexOf(id), 1);

    return true;
}

// ------------------ Helper Functions ------------------ //

function getFile(id)
{
    let index = fileIds.indexOf(id);

    if (index == -1)
        return null;

    return files[index];
}

function fileIdExists(id)
{
    return fileIds.indexOf(id) != -1;
}

// ------------------ Class ------------------ //

class AudioFile
{
    #audioContext;

    /**
     * {AudioBufferSourceNode} #contextBuffer - The audio context for the buffer
     */
    #source;

    /**
     * {AudioBuffer} #contextBuffer - The audio context for the buffer
     */
    #contextBuffer;

    /**
     * {GainNode} #gainNode - The gain node for the buffer
     */
    #gainNode;

    /**
     * {boolean} #resumeFromPause - Whether the audio file should resume from the last position
     */
    #resumeFromPause = false;

    /**
     * {number} #lastPosition - The last position of the audio file
     */
    #lastPosition = 0;

    id;

    volume = 1.0;
    pitch = 1.0;
    doLoop = false;

    buffer;
    channels;
    sampleRate;
    sampleSize;
    hasLoaded = false;

    constructor(id, buffer, channels, sampleRate, sampleSize)
    {
        this.id = id;
        this.buffer = buffer;
        this.channels = channels;
        this.sampleRate = sampleRate;
        this.sampleSize = sampleSize;

        if (buffer && channels && sampleRate && sampleSize)
            this.init();
    }

    /**
     * @todo This method doesn't work yet, OpenAL take my buffer without issue, not the web audio api.
     */
    init()
    {
        this.#audioContext = new (window.AudioContext || window.webkitAudioContext)();

        if (this.#audioContext == null)
            return;

        // set the volume to the volume
        this.#createGainNode();

        // create the buffer
        this.#contextBuffer = this.#audioContext.createBuffer(this.channels, this.buffer.length, this.sampleRate);

        // Send the buffer data to contextBuffer
        for (var channel = 0; channel < this.channels; channel++)
        {
            // This gives us the actual array that contains the data for channel i
            var nowBuffering = this.#contextBuffer.getChannelData(channel);

            // Move the data from the buffer to the contextBuffer
            for (var i = 0; i < this.buffer.length; i++)
            {
                nowBuffering[i] = this.buffer[i * this.channels + channel];
            }
        }

        // prepare the source
        this.#resetSource();

        this.hasLoaded = true;
    }

    // ------------------ Getters and Setters ------------------ //

    get volume()
    {
        return this.volume;
    }

    /**
     * @param {number} volume
     */
    set volume(volume)
    {
        if (this.#gainNode != null)
            this.#createGainNode();

        this.volume = volume;
    }

    get pitch()
    {
        return this.pitch;
    }

    /**
     * @param {number} pitch
     */
    set pitch(pitch)
    {
        if (this.#source != null)
            this.#source.playbackRate.value = pitch;

        this.pitch = pitch;
    }

    get doLoop()
    {
        return this.doLoop;
    }

    /**
     * @param {boolean} doLoop
     */
    set doLoop(doLoop)
    {
        if (this.#source != null)
            this.#source.loop = doLoop;

        this.doLoop = doLoop;
    }

    /**
     * @param {boolean} hasLoaded
     */
    set hasLoaded(hasLoaded)
    {
        this.hasLoaded = hasLoaded;
    }

    get hasLoaded()
    {
        return this.hasLoaded;
    }

    // ------------------ Public Methods ------------------ //

    /**
     * Load the audio file from a binary array
     * Only working method I know for loading audio files as of now
     * @param {Array} binary 
     */
    loadFromBinary(binary)
    {
        if (binary == null)
            return;

        this.#audioContext = new (window.AudioContext || window.webkitAudioContext)();

        if (this.#audioContext == null)
            return;

        // Set the volume
        this.#createGainNode();

        // convert the byte array (binary) to an ArrayBuffer
        var arrayBuffer = new ArrayBuffer(binary.length);
        var view = new Uint8Array(arrayBuffer);

        for (var i = 0; i < binary.length; i++)
        {
            view[i] = binary[i];
        }

        // decode the audio data from the ArrayBuffer
        this.#audioContext.decodeAudioData(arrayBuffer, (buffer) =>
        {
            this.#contextBuffer = buffer;
            this.hasLoaded = true;

            // prepare the source
            this.#resetSource();
        });
    }

    /**
     * Play the audio file
     */
    play()
    {
        if (this.#source != null && this.hasLoaded)
        {

            this.#resetSource();
            this.#audioContext.resume();

            if (this.#resumeFromPause)
            {
                this.#source.start(0, this.#lastPosition);
                this.#resumeFromPause = false;
            }
            else
            {
                this.#source.start();
            }

            console.log("Playing audio file " + this.id);
        }
    }

    /**
     * @todo Test this function
     */
    pause()
    {
        if (this.#source != null && this.hasLoaded)
        {
            this.#lastPosition = this.#audioContext.currentTime;
            this.#resumeFromPause = true;
            this.#source.stop();
        }
    }

    /**
     * Stop the audio file, cannot be resumed, will start from the beginning
     */
    stop()
    {
        if (this.#source != null && this.hasLoaded)
            this.#source.stop();
    }

    // ------------------ Private Methods ------------------ //

    #createGainNode()
    {
        this.#gainNode = this.#audioContext.createGain();
        this.#gainNode.gain.value = this.volume;
        this.#gainNode.connect(this.#audioContext.destination);
    }

    #resetSource()
    {
        this.#source = this.#audioContext.createBufferSource();
        this.#source.buffer = this.#contextBuffer;
        this.#source.playbackRate.value = this.pitch;
        this.#source.loop = this.doLoop;

        this.#source.connect(this.#gainNode);
    }
}