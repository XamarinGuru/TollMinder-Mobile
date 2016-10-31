#!/bin/bash

if [ ! -f /packages/FAKE.4.29.2/tools/FAKE.exe ]; then
 mono --runtime=v4.0 tools/NuGet/nuget.exe install FAKE -Version 4.29.2
fi
mono --runtime=v4.0 packages/FAKE.4.29.2/tools/FAKE.exe build.fsx $@
