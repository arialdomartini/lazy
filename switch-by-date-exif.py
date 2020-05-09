#!python
import PIL.Image
import PIL.ExifTags
import sys
import os
import timestring
import exifread

dry = True

def get_creation_date(filename):

    #with open(filename, 'rb') as fh:
    #    tags = exifread.process_file(fh, stop_tag="EXIF DateTimeOriginal")
    #    dateTaken = tags
    #return dateTaken
    
    im = PIL.Image.open(filename)
    exif = im.getexif()
    creation_time = exif.get(36867)
    datetime_object = timestring.Date(creation_time)
    return datetime_object.date.strftime("%Y-%m-%d")

def get_dir(created_at):
    date_dir = join(working_path, str(created_at))
    if not dry:
        if( not os.path.isdir(date_dir)):
            os.mkdir(date_dir)
    return date_dir
        
def distribute(original_path, created_at):
    destination_dir = get_dir(created_at)
    destination_path = join(destination_dir, original_path)
    print(f"{original_path} => {destination_path}")

    if not dry:
        os.rename(original_path, destination_path)

working_path = sys.argv[1]

from os import listdir
from os.path import isfile, join
files = [ f for f in listdir(working_path) if isfile(join(working_path,f)) ]

for fname in files:
    original_path = join(working_path,fname)
    try:
        created_at = get_creation_date(original_path)
        distribute(original_path, created_at)
    except:
        print(f"*** skipping {fname}")
    #    raise

