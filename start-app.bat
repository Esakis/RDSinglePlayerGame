@echo off
echo ========================================
echo   Red Dragon - Gra RPG
echo ========================================
echo.

echo Starting Backend (.NET API)...
start "RedDragon Backend" cmd /k "cd /d d:\GryWebowe\RDSinglePlayerGame\RedDragonAPI && dotnet run"

echo Waiting for backend to initialize...
timeout /t 8 /nobreak >nul

echo.
echo Starting Frontend (Angular)...
start "RedDragon Frontend" cmd /k "cd /d d:\GryWebowe\RDSinglePlayerGame\RedDragonUI && ng serve"

echo.
echo ========================================
echo   Red Dragon uruchamia sie...
echo ========================================
echo.
echo Backend:  http://localhost:5069
echo Frontend: http://localhost:4200
echo.
echo Press any key to open the game in your browser...
pause >nul

start http://localhost:4200

echo.
echo Both applications are running in separate windows.
echo Close those windows to stop the applications.
echo.
pause
