#!python3
import sys
import os

import datetime
def modification_date(filename):
    t = os.path.getmtime(filename)
    return datetime.date.fromtimestamp(t)

working_path = sys.argv[1]

from os import listdir
from os.path import isfile, join
files = [ f for f in listdir(working_path) if isfile(join(working_path,f)) ]

for fname in files:
    original_path = join(working_path,fname)
    created_at = modification_date(original_path)
    date_dir = join(working_path, str(created_at))
    print(f"{fname} => {date_dir}")
    
    if(True):
        if( not os.path.isdir(date_dir)):
            os.mkdir(date_dir)
        os.rename(fname, join(date_dir, fname))

