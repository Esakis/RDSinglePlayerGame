@echo off
echo ========================================
echo   Click Tracker Application Launcher
echo ========================================
echo.

echo Starting Backend (.NET API)...
start "ClickTracker Backend" cmd /k "cd /d d:\GraWebowa\ClickTrackerAPI && dotnet run"

echo Waiting for backend to initialize...
timeout /t 5 /nobreak >nul

echo.
echo Starting Frontend (Angular)...
start "ClickTracker Frontend" cmd /k "cd /d d:\GraWebowa\ClickTrackerUI && ng serve"

echo.
echo ========================================
echo   Applications are starting...
echo ========================================
echo.
echo Backend:  http://localhost:5069
echo Frontend: http://localhost:4200
echo.
echo Press any key to open the application in your browser...
pause >nul

start http://localhost:4200

echo.
echo Both applications are running in separate windows.
echo Close those windows to stop the applications.
echo.
pause
