FOR /F "skip=1 tokens=1-2 delims=: usebackq" %%G IN ("%ProjectDir%\Files.txt") DO echo "%%H" && copy "%SolutionDir%\%%G\%Outdir%\%%H" .\


echo "Platform: %Platform%, architecture: %PROCESSOR_ARCHITECTURE%"
if "%Platform%" == "x86" (   
  FOR /F "skip=1 tokens=* usebackq" %%G IN ("%ProjectDir%\Files.x86.txt") DO echo "%%G" && copy "%SolutionDir%\%%G" .\
) else if "%Platform%" == "x64" ( 
  FOR /F "skip=1 tokens=* usebackq" %%G IN ("%ProjectDir%\Files.x64.txt") DO echo "%%G" && copy "%SolutionDir%\%%G" .\
) else if "%Platform%" == "AnyCPU" (
  if "%PROCESSOR_ARCHITECTURE%" == "x64" (
  FOR /F "skip=1 tokens=* usebackq" %%G IN ("%ProjectDir%\Files.x64.txt") DO echo "%%G" && copy "%SolutionDir%\%%G" .\
  ) else if "%PROCESSOR_ARCHITECTURE%" == "x86" (
  FOR /F "skip=1 tokens=* usebackq" %%G IN ("%ProjectDir%\Files.x86.txt") DO echo "%%G" && copy "%SolutionDir%\%%G" .\
  ) else (
    echo "ERROR: unknown architecture: "%PROCESSOR_ARCHITECTURE%"
  ) 
) else (
  echo "ERROR: unknown platform: %Platform%"
)

echo "CopyAssemblies done"