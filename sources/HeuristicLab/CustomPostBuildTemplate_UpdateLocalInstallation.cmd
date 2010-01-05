set target=C:\Program Files\HeuristicLab 3.0

copy HeuristicLab.exe "%target%"
copy HeuristicLab.exe.config "%target%"
copy HeuristicLab.PluginInfrastructure.dll "%target%"

FOR /F "skip=1 tokens=1-2 delims=:" %%G IN (Files.txt) DO copy "%SolutionDir%\%%G\%Outdir%\%%H" "%target%"


echo "Platform: %Platform%, architecture: %PROCESSOR_ARCHITECTURE%"
if "%Platform%" == "x86" (   
  FOR /F "skip=1 tokens=*" %%G IN (Files.x86.txt) DO copy "%SolutionDir%\%%G" "%target%"
) else if "%Platform%" == "x64" ( 
  FOR /F "skip=1 tokens=*" %%G IN (Files.x64.txt) DO copy "%SolutionDir%\%%G" "%target%"
) else if "%Platform%" == "AnyCPU" (
  if "%PROCESSOR_ARCHITECTURE%" == "x64" (
  FOR /F "skip=1 tokens=*" %%G IN (Files.x64.txt) DO copy "%SolutionDir%\%%G" "%target%"
  ) else if "%PROCESSOR_ARCHITECTURE%" == "x86" (
  FOR /F "skip=1 tokens=*" %%G IN (Files.x86.txt) DO copy "%SolutionDir%\%%G" "%target%"
  ) else (
    echo "ERROR: unknown architecture: "%PROCESSOR_ARCHITECTURE%"
  ) 
) else (
  echo "ERROR: unknown platform: %Platform%"
)
