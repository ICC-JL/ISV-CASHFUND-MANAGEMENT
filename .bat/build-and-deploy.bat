@echo off

REM =======Change Tracking Instructions========
REM Run the following terminal commands to track/untrack changes to this script. 
REM  - untrack changes : git update-index --no-skip-worktree .bat/build-and-deploy.bat
REM  - track changes   : git update-index --skip-worktree .bat/build-and-deploy.bat


REM ========================================
REM Build and Deploy ATPT Cash Fund Management
REM ========================================

REM -----------------------------------------------------------------------------
REM How to use
REM -----------------------------------------------------------------------------
REM - Run this script from PowerShell or Command Prompt:
REM     C:\Users\<you>\Documents\SourceCode\git\ISV-CASHFUND-MANAGEMENT\.bat\build-and-deploy.bat
REM - Pick a build profile from the menu (1=23R2, 2=24R1, 3=24R2, 4=25R1).
REM - The script will build the selected solution and deploy all project DLLs to
REM   your Acumatica site's Bin folder.
REM - Non-interactive example (24R1):
REM     cmd /c "echo 2| C:\...\ISV-CASHFUND-MANAGEMENT\.bat\build-and-deploy.bat"
REM - If you are using VS Code, you can just run the script from its terminal with .bat\build-and-deploy.bat

REM -----------------------------------------------------------------------------
REM What you may need to update for your local environment
REM -----------------------------------------------------------------------------
REM 1) ACUMATICA_SITE_PATH per profile
REM    - In each :BuildXXXX section, set ACUMATICA_SITE_PATH to your local
REM      Acumatica site's Bin path, e.g.:
REM        C:\AcumaticaSites-C\CFM-2024-R1\Bin
REM    - If left empty (e.g., in 25R1), the script will prompt you at runtime.
REM 2) Optional: BUILD_CONFIG / BUILD_PLATFORM
REM    - Default is Debug / "Any CPU". Change in :BuildProcess if needed.
REM 3) Visual Studio / MSBuild
REM    - VS 2022 must be installed. MSBuild is auto-detected (Enterprise,
REM      Professional, Community, BuildTools, or via vswhere). No changes needed
REM      unless auto-detection fails in your environment.
REM 4) Adding projects later
REM    - The script already deploys DLLs from CashFundManagement and
REM      CashFundPhilippineTax. If you add projects, copy their DLL/PDB collection
REM      blocks by following the existing pattern under "Collect ... assemblies".
REM 5) Permissions
REM    - If your Acumatica site's Bin requires admin rights, run the terminal as
REM      Administrator.


:main
REM ========================================
REM Solution Selection Menu
REM ========================================

echo.
echo ========================================
echo ATPT Cash Fund Management Build Tool
echo ========================================
echo.
echo Please select the build profile:
echo.
echo 1. 23R2
echo 2. 24R1
echo 3. 24R2
echo 4. 25R1
echo 5. Exit
echo.

set /p choice="Enter your choice (1-5): "

if "%choice%"=="1" goto :Build23R2
if "%choice%"=="2" goto :Build24R1
if "%choice%"=="3" goto :Build24R2
if "%choice%"=="4" goto :Build25R1
if "%choice%"=="5" goto :Exit
echo Invalid choice! Please try again.
pause
goto :main

:Build23R2
SET PROJECT_NAME=CashFundManagement
SET SOLUTION_FILE=Build-23R2.sln
SET BUILD_PROFILE=23R2
SET DEBUG_SUBDIR=
SET ACUMATICA_SITE_PATH=C:\AcumaticaSites-C\CFM-2023-R2\Bin
goto :BuildProcess

:Build24R1
SET PROJECT_NAME=CashFundManagement
SET SOLUTION_FILE=Build-24R1.sln
SET BUILD_PROFILE=24R1
SET DEBUG_SUBDIR=
SET ACUMATICA_SITE_PATH=C:\AcumaticaSites-C\CFM-2024-R1\Bin
goto :BuildProcess

:Build24R2
SET PROJECT_NAME=CashFundManagement
SET SOLUTION_FILE=Build-24R2.sln
SET BUILD_PROFILE=24R2
SET DEBUG_SUBDIR=
SET ACUMATICA_SITE_PATH=C:\AcumaticaSites-C\CFM-2024-R2\Bin
goto :BuildProcess

:Build25R1
SET PROJECT_NAME=CashFundManagement
SET SOLUTION_FILE=Build-25R1.sln
SET BUILD_PROFILE=25R1
SET DEBUG_SUBDIR=\25R1
SET ACUMATICA_SITE_PATH=
goto :BuildProcess

:Exit
echo Exiting...
exit /b 0

:BuildProcess

SET BUILD_CONFIG=Debug
SET BUILD_PLATFORM=Any CPU

REM Check if ACUMATICA_SITE_PATH is empty and prompt user
if "%ACUMATICA_SITE_PATH%"=="" (
    echo.
    echo ========================================
    echo ACUMATICA SITE PATH REQUIRED
    echo ========================================
    echo.
    echo No deployment path has been specified.
    echo Please enter the path to your Acumatica site's Bin directory.
    echo.
    echo Example paths:
    echo   C:\AcumaticaSites-C\CFM-2024-R1\Bin
    echo   C:\AcumaticaSites-C\REDPtax\Bin
    echo.
    set /p ACUMATICA_SITE_PATH="Enter Acumatica site path: "
    echo.
)

REM Resolve MSBuild path (auto-detect common VS2022 installs, then vswhere)
SET "MSBUILD_PATH="

REM Try common Visual Studio 2022 editions
SET "CANDIDATE_MSBUILD=C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
IF EXIST "%CANDIDATE_MSBUILD%" SET "MSBUILD_PATH=%CANDIDATE_MSBUILD%"

IF NOT DEFINED MSBUILD_PATH (
    SET "CANDIDATE_MSBUILD=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    IF EXIST "%CANDIDATE_MSBUILD%" SET "MSBUILD_PATH=%CANDIDATE_MSBUILD%"
)

IF NOT DEFINED MSBUILD_PATH (
    SET "CANDIDATE_MSBUILD=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    IF EXIST "%CANDIDATE_MSBUILD%" SET "MSBUILD_PATH=%CANDIDATE_MSBUILD%"
)

IF NOT DEFINED MSBUILD_PATH (
    SET "CANDIDATE_MSBUILD=C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
    IF EXIST "%CANDIDATE_MSBUILD%" SET "MSBUILD_PATH=%CANDIDATE_MSBUILD%"
)

REM Fallback: use vswhere to locate MSBuild dynamically
IF NOT DEFINED MSBUILD_PATH (
    IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
        FOR /F "usebackq tokens=*" %%i IN (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) DO (
            SET "MSBUILD_PATH=%%i"
            GOTO :AfterMsbuildDetect
        )
    )
)
:AfterMsbuildDetect

REM ========================================
REM Build Process
REM ========================================

echo.
echo ========================================
echo Building %PROJECT_NAME%...
echo ========================================
echo Solution: %SOLUTION_FILE%
echo Configuration: %BUILD_CONFIG%
echo Platform: %BUILD_PLATFORM%
if defined BUILD_PROFILE echo Build Profile: %BUILD_PROFILE%
echo.

REM Check if MSBuild exists
if not exist "%MSBUILD_PATH%" (
    echo ERROR: MSBuild not found at %MSBUILD_PATH%
    echo Please verify Visual Studio 2022 installation or update MSBUILD_PATH variable
    pause
    exit /b 1
)

REM Build the solution
echo Using MSBuild at: %MSBUILD_PATH%
"%MSBUILD_PATH%" "%SOLUTION_FILE%" /restore /p:Configuration=%BUILD_CONFIG% /p:Platform="%BUILD_PLATFORM%" /verbosity:minimal

if %ERRORLEVEL% neq 0 (
    echo.
    echo ERROR: Build failed!
    pause
    exit /b 1
)

echo.
echo Build completed successfully!

REM ========================================
REM Deployment Process
REM ========================================

echo.
echo ========================================
echo Deploying to Acumatica site...
echo ========================================
echo Target path: %ACUMATICA_SITE_PATH%
echo.

REM Check if target directory exists
if not exist "%ACUMATICA_SITE_PATH%" (
    echo ERROR: Target directory does not exist: %ACUMATICA_SITE_PATH%
    echo Please verify ACUMATICA_SITE_PATH variable
    pause
    exit /b 1
)

REM Create temporary deployment folder
SET TEMP_DEPLOY_FOLDER=%TEMP%\ATPT_Deploy_%BUILD_PROFILE%
if exist "%TEMP_DEPLOY_FOLDER%" rmdir /s /q "%TEMP_DEPLOY_FOLDER%"
mkdir "%TEMP_DEPLOY_FOLDER%"

echo Collecting built assemblies...
echo Temporary deployment folder: %TEMP_DEPLOY_FOLDER%
echo.

REM Collect CashFundManagement DLLs
SET CFM_SOURCE_PATH=CashFundManagement\bin\%BUILD_CONFIG%
if "%BUILD_CONFIG%"=="Debug" (
    SET CFM_SOURCE_PATH=%CFM_SOURCE_PATH%%DEBUG_SUBDIR%
)

echo Collecting CashFundManagement assemblies from: %CFM_SOURCE_PATH%
if exist "%CFM_SOURCE_PATH%\CashFundManagement.dll" (
    copy "%CFM_SOURCE_PATH%\CashFundManagement.dll" "%TEMP_DEPLOY_FOLDER%\"
    echo   - CashFundManagement.dll
) else (
    echo WARNING: CashFundManagement.dll not found at %CFM_SOURCE_PATH%
)

if exist "%CFM_SOURCE_PATH%\CashFundManagement.pdb" (
    copy "%CFM_SOURCE_PATH%\CashFundManagement.pdb" "%TEMP_DEPLOY_FOLDER%\"
    echo   - CashFundManagement.pdb
)

REM Collect CashFundPhilippineTax DLLs
SET CFPT_SOURCE_PATH=CashFundPhilippineTax\bin\%BUILD_CONFIG%
if "%BUILD_CONFIG%"=="Debug" (
    SET CFPT_SOURCE_PATH=%CFPT_SOURCE_PATH%%DEBUG_SUBDIR%
)

echo Collecting CashFundPhilippineTax assemblies from: %CFPT_SOURCE_PATH%
if exist "%CFPT_SOURCE_PATH%\CashFundPhilippineTax.dll" (
    copy "%CFPT_SOURCE_PATH%\CashFundPhilippineTax.dll" "%TEMP_DEPLOY_FOLDER%\"
    echo   - CashFundPhilippineTax.dll
) else (
    echo WARNING: CashFundPhilippineTax.dll not found at %CFPT_SOURCE_PATH%
)

if exist "%CFPT_SOURCE_PATH%\CashFundPhilippineTax.pdb" (
    copy "%CFPT_SOURCE_PATH%\CashFundPhilippineTax.pdb" "%TEMP_DEPLOY_FOLDER%\"
    echo   - CashFundPhilippineTax.pdb
)

REM Check if any DLLs were collected
dir /b "%TEMP_DEPLOY_FOLDER%\*.dll" >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo.
    echo ERROR: No DLL files were found to deploy!
    echo Please check build output paths
    pause
    exit /b 1
)

echo.
echo Deploying all assemblies to target site...
copy "%TEMP_DEPLOY_FOLDER%\*.*" "%ACUMATICA_SITE_PATH%\"

if %ERRORLEVEL% neq 0 (
    echo.
    echo ERROR: Failed to copy assemblies to target location!
    pause
    exit /b 1
)

REM Clean up temporary folder
rmdir /s /q "%TEMP_DEPLOY_FOLDER%"

echo.
echo ========================================
echo Deployment completed successfully!
echo ========================================
echo.
echo All project assemblies have been deployed to:
echo %ACUMATICA_SITE_PATH%
echo.
echo Deployed assemblies:
for %%f in ("%ACUMATICA_SITE_PATH%\CashFund*.dll") do echo   - %%~nxf
echo.
echo Build Configuration: %BUILD_CONFIG%
echo Build Platform: %BUILD_PLATFORM%
echo Build Profile: %BUILD_PROFILE%
echo.

pause