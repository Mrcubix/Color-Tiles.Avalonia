#!/bin/bash

# Build game for all Desktop Platforms
./build-desktop-all.sh

# Build game for all Android Architectures
./build-android-all.sh

# Browser
dotnet publish ColorTiles.Browser -c Release -o build/browser