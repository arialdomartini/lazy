#!/usr/bin/env bash
set -euo pipefail

mkdir -p jpg
for file in *.HEIC; do
    echo $file
    magick convert "$file" "jpg/${file%.HEIC}.jpg"
done
