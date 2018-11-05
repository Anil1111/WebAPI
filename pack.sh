#!/bin/bash

FNAME="`date '+%Y%m%d_%H%M'`_WebAPI.tgz"
EX="--exclude=WebAPI/obj "
EX="${EX} --exclude=WebAPI/bin "
#EX="${EX} --exclude=WebAPI.Tests/bin "
#EX="${EX} --exclude=WebAPI.Tests/obj "

set -x
tar -czf $FNAME $EX .vs WebAPI WebAPI.sln pack.sh browsing.sh .gitignore .gitattributes
set +x

