@echo off
call C:\Programmi\Mono-1.1.15\bin\setmonopath.bat
cd /D C:\Mojhy\WebProject\Mojhy
xsp2 --root . --port 8088 --applications /:.
