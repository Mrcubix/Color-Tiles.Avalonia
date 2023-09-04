# Color Tiles in Avalonia

## Premises

This is a port of a clone of mine made in Godot 4 using C#, You can find the original [here](https://github.com/Mrcubix/Color-Tiles).

The rest of this README is the same as the original, for now.

## Introduction

This is a modern clone of the classic game Color Tiles from [en.gamesaien.com](https://en.gamesaien.com/).
This game is made in Godot 4 using C# as it is my main language.

## Dependencies

- .NET 6 **Desktop Runtime**: [Link](https://dotnet.microsoft.com/en-us/download/dotnet/6.0#:~:text=x86-,.NET%20Desktop%20Runtime%206.0.21,-The%20.NET%20Desktop) | [Fallback Link](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

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
- Open the project in Godot 4
- You can either start the game from the editor or export it to your platform of choice

## Is there anything i can do to help?

There are a couple things that are missing, like:

- Translation support, although the game should be auto-translated, it may not be of the best quality.
Some Sprites may also need to be remade to suit these translations,
- Some audio files are missing for whenever tiles are removed or the game is over, ideally, those should be made from scratch, nothing too complex,
- A better icon for the game,
- Maybe a better tileset,
- ~~A reset button if you want to retry spam like ed~~   
![bmc](https://cdn.7tv.app/emote/643ce6a2ce9e08be709d62c1/4x.webp)