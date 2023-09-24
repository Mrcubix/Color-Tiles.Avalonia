# Color Tiles in Avalonia (Port) (Work In Progress)

## Premises

This is a port of a clone of mine made in Godot 4 using C#, You can find the original [here](https://github.com/Mrcubix/Color-Tiles).

## Introduction

This is a modern clone of the classic game Color Tiles from [en.gamesaien.com](https://en.gamesaien.com/).
This game is made in Avalonia.

## Dependencies

- .NET 7 **Desktop Runtime**: [Link](https://dotnet.microsoft.com/en-us/download/dotnet/7.0#runtime-desktop-7.0.10:~:text=x86-,.NET%20Desktop%20Runtime,-7.0.10) | [Fallback Link](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Play Online

You can play the game online, in your browser [here](https://mrcubix.github.io/Color-Tiles.Avalonia/).

## Download

You can get the latest release of the game [here](https://github.com/Mrcubix/Color-Tiles/releases/latest).

## How to play

- Click on an empty tile,
- if there are any tiles vertically or horizontally with the same color, they will be removed,
- if there are none, you loose 10 seconds of time,
- the goal of this game is to get the highest score possible in 120 seconds.

![Instruction image from gamesaien.com](https://en.gamesaien.com/game/color_tiles/color_tiles_zu01.png)

## Previews

Main Menu:

![Image of the main menu](https://i.imgur.com/nR1o1Gd.png)

Gameplay:

![Image of an ongoing game](https://i.imgur.com/uUooXwI.png)

Game Over:

![Image of the game over screen](https://i.imgur.com/975m1ft.png)

## How to Install

This is a self-contained application, there are no installers provided.
Packages for linux are not available yet, don't know enough to make them.

**Note**: For windows & linux, make sure you extract the zip file, ideally in its own folder.

You should have something like this once extracted:

![Image of file structure](https://i.imgur.com/GjcVF8N.png)

For MacOS users, i'm sorry but you will have to deal with Apple's Gatekeeper.

## How to build

- Clone this repository
- Open the project with your terminal of choice
- Copy the followings :
```bash
dotnet restore
// Building for windows
dotnet publish ColorTiles.Desktop -c Release -r win-x64 -p:PublishSingleFile=true
// Building for linux
dotnet publish ColorTiles.Desktop -c Release -r linux-x64 -p:PublishSingleFile=true
```
- You can then start the game using the built Release binaries

## Is there anything i can do to help? (WIP)

There are a couple things that are missing, like:

- (Partially Complete) Translation support.
Some Sprites may also need to be remade to suit these translations,
- ~~Some audio files are missing for whenever tiles are removed or the game is over, ideally, those should be made from scratch, nothing too complex~~,
- A better icon for the game,
- Maybe a better tileset,
- If you have some experience in this domain, I may accept suggestions that improve the codebase,
- ~~A reset button if you want to retry spam like ed~~   
![bmc](https://cdn.7tv.app/emote/643ce6a2ce9e08be709d62c1/4x.webp)