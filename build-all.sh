#!/bin/bash

# Build game for all Desktop Platforms
./build-desktop-all.sh

# Build game for all Android Architectures
./build-android-all.sh

# Browser
dotnet publish ColorTiles.Browser -c Release
#cp ColorTiles.Browser/bin/Release/net7.0/browser-wasm build/browser -r