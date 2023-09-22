#!/bin/bash



# Windows x64
dotnet publish ColorTiles.Desktop -c Release -r win-x64 --no-self-contained -o build/win-x64 -p:IncludeWindowsNativeLibs=true

# Windows x86
dotnet publish ColorTiles.Desktop -c Release -r win-x86 --no-self-contained -o build/win-x86 -p:IncludeWindowsNativeLibs=true

# Linux x64
dotnet publish ColorTiles.Desktop -c Release -r linux-x64 --no-self-contained -o build/linux-x64

# Linux arm (32 Bits)
dotnet publish ColorTiles.Desktop -c Release -r linux-arm --no-self-contained -o build/linux-arm

# Linux arm64
dotnet publish ColorTiles.Desktop -c Release -r linux-arm64 --no-self-contained -o build/linux-arm64