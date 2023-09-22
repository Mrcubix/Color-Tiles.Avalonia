#!/bin/bash

# Android x64
dotnet publish ColorTiles.Android -c Release -r android-x64 -o build/android-x64 -p:IncludeAndroidNativeLibs=true

# Android X86
dotnet publish ColorTiles.Android -c Release -r android-x86 -o build/android-x86 -p:IncludeAndroidNativeLibs=true

# Android arm64
dotnet publish ColorTiles.Android -c Release -r android-arm64 -o build/android-arm64 -p:IncludeAndroidNativeLibs=true

# Android arm
dotnet publish ColorTiles.Android -c Release -r android-arm -o build/android-arm -p:IncludeAndroidNativeLibs=true

