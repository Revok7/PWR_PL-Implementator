@echo off
chcp 65001

::Debug
if exist "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" (
  xcopy "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" "PWR_PL-Implementator\bin\Debug\net6.0\" /Y
)
if exist "skrypt uruchamiający deimplementator\Deimplementuj_PWR_PL.cmd" (
  xcopy "skrypt uruchamiający deimplementator\Deimplementuj_PWR_PL.cmd" "PWR_PL-Implementator\bin\Debug\net6.0\" /Y
)
if exist "skrypt uruchamiający implementator\Zaimplementuj_PWR_PL.cmd" (
  xcopy "skrypt uruchamiający implementator\Zaimplementuj_PWR_PL.cmd" "PWR_PL-Implementator\bin\Debug\net6.0\" /Y
)

::Release_publish
if exist "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" (
  xcopy "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" "PWR_PL-Implementator\bin\Release_publish\net6.0\" /Y
)
if exist "skrypt uruchamiający deimplementator\Deimplementuj_PWR_PL.cmd" (
  xcopy "skrypt uruchamiający deimplementator\Deimplementuj_PWR_PL.cmd" "PWR_PL-Implementator\bin\Release_publish\net6.0\" /Y
)
if exist "skrypt uruchamiający deimplementator\Deimplementuj_PWR_PL.cmd" (
  xcopy "skrypt uruchamiający deimplementator\Deimplementuj_PWR_PL.cmd" "PWR_PL-Implementator\bin\Release_publish\net6.0\" /Y
)
if exist "skrypt uruchamiający implementator\Zaimplementuj_PWR_PL.cmd" (
  xcopy "skrypt uruchamiający implementator\Zaimplementuj_PWR_PL.cmd" "PWR_PL-Implementator\bin\Release_publish\net6.0\" /Y
)

exit


