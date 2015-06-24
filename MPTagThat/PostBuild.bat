REM %1 = Solution Directory
REM %2 = $(ConfigurationName) Debug/Release

REM Hack to remove dll not needed in root 
del *.dll
del *.pdb

REM Copy All new files from base
xcopy %1\MPTagThat.Base\*.* . /E /R /Y /D

REM Copy Core 
xcopy /y %1\MPTagThat.Core\bin\%2\MPTagThat.Core.* .

REM Copy Taglib-Sharp 
xcopy /y %1\Libraries\taglib-sharp\bin\%2\taglib-sharp.dll .\bin
xcopy /y %1\Libraries\taglib-sharp\bin\%2\ICSharpCode.SharpZipLib.dll .\bin

REM Copy Lyricsengine 
xcopy /y %1\Libraries\LyricsEngine\bin\%2\LyricsEngine.dll .\bin
xcopy /y %1\Libraries\LyricsEngine\bin\%2\LyricsEngine.dll.* .\bin

REM Copy Gain dll 
xcopy /y %1\Libraries\gain\bin\%2\gain.dll .\bin

REM Copy FreeImage Library
xcopy /y %1\Libraries\FreeImage\bin\%2\FreeImageNET.dll .\bin

REM Copy DiscogsNet Library
xcopy /y %1\Libraries\DiscogsNet\bin\%2\DiscogsNET.dll .\bin
xcopy /y %1\Libraries\DiscogsNet\bin\%2\Newtonsoft.Json.dll .\bin