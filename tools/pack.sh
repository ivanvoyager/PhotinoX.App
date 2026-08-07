#!/usr/bin/env bash
set -euo pipefail

scriptDir="$(cd "$(dirname "$0")" && pwd)"
csproj="$scriptDir/../src/PhotinoX.App/PhotinoX.App.csproj"
configuration="Release"
outDir="$scriptDir"

dotnet clean "$csproj" -c "$configuration"
dotnet pack "$csproj" -c "$configuration" -o "$outDir"