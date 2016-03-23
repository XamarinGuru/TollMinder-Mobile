#!/bin/bash

if [ ! -f /packages/FAKE.4.22.6/tools/FAKE.exe ]; then
 mono --runtime=v4.0 tools/NuGet/nuget.exe install FAKE -Version 4.22.6
fi
mono --runtime=v4.0 packages/FAKE.4.12.0/tools/FAKE.exe build.fsx $@
