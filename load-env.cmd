@echo off

if "%~1"=="" (
    echo Please provide the path to your .env file.
    echo Usage: load-env.cmd \path\to\.env
    exit /b 1
)


for /f "usebackq tokens=1,* delims==" %%A in ("%~1") do (
    set %%A=%%B
    echo %%A==%%B
)