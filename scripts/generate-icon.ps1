$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing
$root = Split-Path -Parent $PSScriptRoot
$assets = Join-Path $root "TaskManagerApp\TaskManagerApp\Assets"
New-Item -ItemType Directory -Path $assets -Force | Out-Null
$size = 256
$bitmap = New-Object System.Drawing.Bitmap $size, $size
$graphics = [System.Drawing.Graphics]::FromImage($bitmap)
$graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$graphics.Clear([System.Drawing.Color]::FromArgb(7, 6, 17))
$violet = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(155, 123, 255))
$cyan = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(105, 230, 255)), 8
$ring = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(120, 155, 123, 255)), 5
$graphics.FillEllipse($violet, 91, 91, 74, 74)
$graphics.DrawEllipse($ring, 43, 92, 170, 72)
$graphics.DrawLine($cyan, 37, 185, 219, 71)
$graphics.DrawLine($cyan, 48, 200, 208, 56)
$pngPath = Join-Path $assets "pulsar-icon.png"
$bitmap.Save($pngPath, [System.Drawing.Imaging.ImageFormat]::Png)
$png = [IO.File]::ReadAllBytes($pngPath)
$stream = New-Object IO.MemoryStream
$writer = New-Object IO.BinaryWriter $stream
$writer.Write([uint16]0); $writer.Write([uint16]1); $writer.Write([uint16]1)
$writer.Write([byte]0); $writer.Write([byte]0); $writer.Write([byte]0); $writer.Write([byte]0)
$writer.Write([uint16]1); $writer.Write([uint16]32); $writer.Write([uint32]$png.Length); $writer.Write([uint32]22); $writer.Write($png)
[IO.File]::WriteAllBytes((Join-Path $assets "pulsar.ico"), $stream.ToArray())
$writer.Dispose(); $stream.Dispose(); $ring.Dispose(); $cyan.Dispose(); $violet.Dispose(); $graphics.Dispose(); $bitmap.Dispose()
