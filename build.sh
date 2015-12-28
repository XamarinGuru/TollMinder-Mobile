#!/bin/bash

mono --runtime=v4.0 tools/NuGet/nuget.exe install FAKE -Version 4.4.2
mono --runtime=v4.0 packages/FAKE.4.4.2/tools/FAKE.exe build.fsx $@
