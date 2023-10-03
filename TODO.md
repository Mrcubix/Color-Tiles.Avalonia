### UI

- [x] Display for the color tiles to sit on until nuked
    - [x] Build an image or,
    - ~~Use an UniformGrid properly somehow~~ to use UniformGrid properly, it need ot be implemented properly in the first place

- [x] HUD
    - [x] Health
        - [x] Might create a custom control for this
    - [x] Score
        - [x] Simple binding to the score property
    - [x] Reset button
        - [x] Go back to the main menu

- [x] Feed the final score to the game over screen

- [x] Look into overriding the Button Control to add hover SFXs

- [x] Fix the Quit button on Single View Platforms (Android & Browser)

- [ ] Fix Text Scaling on the gameOverScreen

- [ ] Fix button scaling on the HUD

- [ ] Look into finding a way to add a button to enter fullscreen for browser & desktop users

### Audio

- [ ] Look into making audio for the following:
    - [x] When a button is clicked
    - [x] When a button is hovered over
    - [x] When tiles are removed
    - [x] When a penalty is applied
    - [ ] When the game is over

- [x] Fix audio not working in Release builds
    
- [x] Make audio supported on all **proper** platforms
    - [x] Override methods in WebAudio File (& move it to ColorTiles.Browser instead)
    - [x] Pass in the entire audio file as a byte array instead of just the raw data to simplify the process
          (Struggling to make it work with just the raw audio data like i do using OpenAL)

- [x] Make the Switch to a newer version of OpenTK whenever platform detection is fixed for OpenAL on Android devices & others

- � Try to not crash the game when audio cannot be initialized

### Misc

- [ ] Clean up the MainViewModel Class
- [ ] [Partially Complete] Look into using Dependency Injection for default Audiosets / Tilesets
- [x] Look into using Dependency Injection for quitting the game using platform native implementations