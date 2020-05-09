#!/bin/bash
for f in *.heic
do
echo "Working on file $f"
heif-convert $f $f.jpg
done
