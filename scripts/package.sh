#!/usr/bin/env bash
set -euo pipefail
RUNTIME="${1:-linux-x64}"
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"
DESTINATION="artifacts/Violet-Pulsar-$RUNTIME"
rm -rf "$DESTINATION"
dotnet publish ./TaskManagerApp/TaskManagerApp/TaskManagerApp.csproj \
  --configuration Release --runtime "$RUNTIME" --self-contained true --output "$DESTINATION" \
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:IncludeAllContentForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=None
cp README.md LICENSE "$DESTINATION/"
cp -R docs "$DESTINATION/"
chmod +x "$DESTINATION/VioletPulsar"
"$DESTINATION/VioletPulsar" --diagnostics
printf '%s\n' "$DESTINATION"
