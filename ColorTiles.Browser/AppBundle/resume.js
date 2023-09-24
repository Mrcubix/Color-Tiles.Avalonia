let resumed = false;

document.addEventListener("click", () => {
    if (!resumed) {
        resumed = true;
        let audioContext = new (window.AudioContext || window.webkitAudioContext)();
        audioContext.resume();
    }
});