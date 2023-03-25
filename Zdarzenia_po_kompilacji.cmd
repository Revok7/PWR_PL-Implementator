@echo off
chcp 65001

::Debug
xcopy "!!!plik_konfiguracyjny_autokopiowany_po_kompilacji.json" "PWR_PL-Implementator\bin\Debug\net6.0" /Y
if exist "PWR_PL-Implementator\bin\Debug\net6.0\!!!plik_konfiguracyjny_autokopiowany_po_kompilacji.json" (
   move /Y "PWR_PL-Implementator\bin\Debug\net6.0\!!!plik_konfiguracyjny_autokopiowany_po_kompilacji.json" "PWR_PL-Implementator\bin\Debug\net6.0\cfg.json"
)
if exist "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" (
  xcopy "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" "PWR_PL-Implementator\bin\Debug\net6.0\" /Y
)

::Release_publish
xcopy "!!!plik_konfiguracyjny_autokopiowany_po_kompilacji.json" "PWR_PL-Implementator\bin\Release_publish\net6.0\PWR_PL-Implementator_e1ps_winx64\" /Y
if exist "PWR_PL-Implementator\bin\Release_publish\net6.0\PWR_PL-Implementator_e1ps_winx64\!!!plik_konfiguracyjny_autokopiowany_po_kompilacji.json" (
   move /Y "PWR_PL-Implementator\bin\Release_publish\net6.0\PWR_PL-Implementator_e1ps_winx64\!!!plik_konfiguracyjny_autokopiowany_po_kompilacji.json" "PWR_PL-Implementator\bin\Release_publish\net6.0\PWR_PL-Implementator_e1ps_winx64\cfg.json"
)
if exist "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" (
  xcopy "kompatybilność z win7-x64\api-ms-win-core-winrt-l1-1-0.dll" "PWR_PL-Implementator\bin\Release_publish\net6.0\PWR_PL-Implementator_e1ps_winx64\" /Y
)




