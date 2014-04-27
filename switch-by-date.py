#!/usr/local/opt/pyenv/shims/python
import sys
import os

import datetime
def modification_date(filename):
    t = os.path.getmtime(filename)
    return datetime.date.fromtimestamp(t)

mypath = sys.argv[1]

from os import listdir
from os.path import isfile, join
files = [ f for f in listdir(mypath) if isfile(join(mypath,f)) ]

for fname in files:
    print(fname)
    f = join(mypath,fname)
    created_at = modification_date(f)
    dird = join(mypath, str(created_at)) 
    if( not os.path.isdir(dird)):
        os.mkdir(dird)
    os.rename(f, join(dird, fname))
