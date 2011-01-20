REM %1 = Solution Directory
REM %2 = $(ConfigurationName) Debug/Release

REM Hack to remove dll not needed in root 
del *.dll

REM Copy All new files from base
xcopy %1\MPTagThat.Base\*.* . /E /R /Y /D

REM Copy Core 
xcopy /y %1\MPTagThat.Core\bin\%2\MPTagThat.Core.* .

REM Copy Taglib-Sharp 
xcopy /y %1\taglib-sharp\bin\%2\taglib-sharp.dll .\bin
xcopy /y %1\taglib-sharp\bin\%2\ICSharpCode.SharpZipLib.dll .\bin

REM Copy Lyricsengine 
xcopy /y %1\LyricsEngine\bin\%2\LyricsEngine.dll .\bin
xcopy /y %1\LyricsEngine\bin\%2\LyricsEngine.dll.* .\bin

REM Copy Gain dll 
xcopy /y %1\gain\bin\%2\gain.dll .\bin