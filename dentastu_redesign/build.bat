@echo off
rem [AI-generated] Build script for the redesigned UI (uses csc.exe bundled with .NET Framework 4.x; no Visual Studio needed)
rem The calculation core reuses the original ..\dentastu\DK.cs and ..\dentastu\Polynomial.cs unchanged.
setlocal
cd /d %~dp0
set CSC=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe
if not exist bin mkdir bin
"%CSC%" -nologo -codepage:65001 -target:winexe -optimize+ -out:bin\dentastu_redesign.exe ^
  -r:System.Numerics.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll ^
  src\*.cs ..\dentastu\DK.cs ..\dentastu\Polynomial.cs
if errorlevel 1 exit /b 1
echo built: bin\dentastu_redesign.exe
