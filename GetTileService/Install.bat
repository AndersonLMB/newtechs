%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe C:\Users\lmb\source\repos\NewTechs\GetTileService\bin\Debug\GetTileService.exe
Net Start GetTileService
sc config GetTileService start= auto
pause