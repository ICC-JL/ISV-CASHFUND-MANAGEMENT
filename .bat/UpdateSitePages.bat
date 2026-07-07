@echo off
setlocal ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

REM =======Change Tracking Instructions========
REM Run the following terminal commands to track/untrack changes to this script. 
REM  - track changes : git update-index --no-skip-worktree .bat/UpdateSitePages.bat
REM  - untrack changes : git update-index --skip-worktree .bat/UpdateSitePages.bat

REM ========================================
REM Update Site Pages - ATPT (Multi-version)
REM Copy ASPX pages to a selected Acumatica site version
REM ========================================
REM
REM USAGE INSTRUCTIONS:
REM -------------------
REM This script copies ASPX and ASPX.CS files from the ProjectSourceControl folder
REM to your local Acumatica site's Pages folder for testing.
REM
REM Method 1: Run without parameters (Interactive Menu)
REM   - Double-click UpdateSitePages.bat
REM   - Select version from menu (1-5)
REM
REM Method 2: Run with version parameter (Command Line)
REM   - UpdateSitePages.bat 24R1
REM   - UpdateSitePages.bat 25R1
REM   Supported versions: 23R2, 24R1, 24R2, 25R1, 25R2
REM
REM Method 3: Execute on VS Code Terminal or Command Prompt .bat\UpdateSitePages.bat
REM
REM REQUIRED CONFIGURATION (Must be updated for each developer):
REM -------------------------------------------------------------
REM 1. REPO_ROOT: Path to your Git repositories folder
REM    Default: C:\Users\renor\Documents\SourceCode\git
REM    Update to: Your local Git repository root path
REM
REM 2. SITES_ROOT: Path to your Acumatica sites folder
REM    Default: C:\AcumaticaSites-C
REM    Update to: Your local Acumatica installation root
REM
REM 3. PROJECT_NAME: Name pattern of the customization project
REM    Default: CashFundManagement.YYYY.MM.DD
REM    Note: This must match the folder name in ProjectSourceControl\[Version]\
REM
REM 4. SITE_FOLDER mappings: Site folder names for each version
REM    Defaults: CFM-2023-R2, CFM-2024-R1, CFM-2024-R2, etc.
REM    Update to: Your local site folder names if different
REM
REM 5. PAGE_FOLDER: Subfolder under Pages where ASPX files are located
REM    Default: ATPT
REM    Update if: Using a different page folder name
REM
REM WHAT THIS SCRIPT DOES:
REM ----------------------
REM - Prompts for Acumatica version selection (if not provided as parameter)
REM - Validates source and target directories
REM - Creates target directory if it doesn't exist
REM - Copies only NEW or MODIFIED ASPX and CS files (incremental copy using robocopy)
REM - Does NOT overwrite newer destination files
REM - Displays summary and next steps
REM
REM AFTER RUNNING THIS SCRIPT:
REM --------------------------
REM 1. Restart IIS or recycle application pool:
REM    - Open IIS Manager
REM    - Select your Acumatica site
REM    - Click "Recycle" in Application Pools
REM 2. Test pages in browser
REM 3. Verify all functionality works as expected
REM
REM TROUBLESHOOTING:
REM ----------------
REM - "Source directory does not exist": Check REPO_ROOT and PROJECT_NAME variables
REM - "Failed to create target directory": Check SITES_ROOT and folder permissions
REM - Files not copying: Verify source files exist in ProjectSourceControl\[Version]\
REM ========================================

REM Usage: optional version argument
REM   %~1 -> Version to update: 23R2 | 24R1 | 24R2 | 25R1 | 25R2
REM If omitted, a menu will prompt for selection.

REM ========================================
REM CONFIGURATION VARIABLES
REM ========================================
REM >>> UPDATE THESE VARIABLES FOR YOUR LOCAL ENVIRONMENT <<<

REM Project name pattern (must match folder in ProjectSourceControl\[Version]\)
SET PROJECT_NAME=CashFundManagement.YYYY.MM.DD

REM Version to update (set by menu or command-line parameter)
SET ACUMATICA_VERSION=

REM Page folder name under Pages\ (e.g., ATPT, Custom, etc.)
SET PAGE_FOLDER=ATPT

REM ========================================
REM PATH CONFIGURATION
REM ========================================
REM >>> DEVELOPERS: UPDATE THESE PATHS TO MATCH YOUR LOCAL SETUP <<<

REM Root path to your Git repositories folder
REM Example: C:\Users\YourName\Documents\Git
REM Example: C:\Source\Git
REM Example: D:\Development\Repositories
SET REPO_ROOT=C:\Users\renor\Documents\SourceCode\git

REM Root path to your Acumatica sites folder
REM Example: C:\inetpub\wwwroot
REM Example: C:\AcumaticaSites
REM Example: D:\Acumatica\Sites
SET SITES_ROOT=C:\AcumaticaSites-C

REM ========================================
REM DO NOT MODIFY BELOW THIS LINE
REM (Unless you need to add new version mappings)
REM ========================================

IF NOT "%~1"=="" (
    SET ACUMATICA_VERSION=%~1
    goto :PostVersionSelection
)

:VersionSelection
echo.
echo ========================================
echo ATPT Update Pages - Version Selection
echo ========================================
echo.
echo Please select Acumatica version to update:
echo.
echo 1. 23R2
echo 2. 24R1
echo 3. 24R2
echo 4. 25R1
echo 5. 25R2
echo 0. Exit
echo.
set /p vChoice="Enter your choice (0-5): "

IF "%vChoice%"=="0" (
    echo.
    echo Exiting script...
    exit /b 0
)
IF "%vChoice%"=="1" SET ACUMATICA_VERSION=23R2
IF "%vChoice%"=="2" SET ACUMATICA_VERSION=24R1
IF "%vChoice%"=="3" SET ACUMATICA_VERSION=24R2
IF "%vChoice%"=="4" SET ACUMATICA_VERSION=25R1
IF "%vChoice%"=="5" SET ACUMATICA_VERSION=25R2

IF "%ACUMATICA_VERSION%"=="" (
    echo Invalid choice! Please try again.
    pause
    goto :VersionSelection
)

REM Define source control folder based on version
REM 23R2 uses its own folder; 24R1+ all use 24R1 folder (AOTM - backward compatible)
SET CONTROL_FOLDER=%ACUMATICA_VERSION%
IF /I "%ACUMATICA_VERSION%"=="24R2" SET CONTROL_FOLDER=24R1
IF /I "%ACUMATICA_VERSION%"=="25R1" SET CONTROL_FOLDER=24R1
IF /I "%ACUMATICA_VERSION%"=="25R2" SET CONTROL_FOLDER=24R1

:PostVersionSelection

REM ========================================
REM PATH CONSTRUCTION
REM ========================================

REM Source path (uses ProjectSourceControl subfolder matching version)
SET SOURCE_PAGES_PATH=%REPO_ROOT%\ISV-CASHFUND-MANAGEMENT\ProjectSourceControl\%CONTROL_FOLDER%\%PROJECT_NAME%\Pages\%PAGE_FOLDER%

REM ========================================
REM SITE FOLDER MAPPING
REM ========================================
REM >>> DEVELOPERS: UPDATE THESE IF YOUR SITE FOLDER NAMES ARE DIFFERENT <<<
REM Map version to site folder name under SITES_ROOT
REM Format: IF /I "%ACUMATICA_VERSION%"=="[version]" SET SITE_FOLDER=[your-folder-name]

SET SITE_FOLDER=
IF /I "%ACUMATICA_VERSION%"=="23R2" SET SITE_FOLDER=CFM-2023-R2
IF /I "%ACUMATICA_VERSION%"=="24R1" SET SITE_FOLDER=CFM-2024-R1
IF /I "%ACUMATICA_VERSION%"=="24R2" SET SITE_FOLDER=CFM-2024-R2
IF /I "%ACUMATICA_VERSION%"=="25R1" SET SITE_FOLDER=CFM-2025-R1
IF /I "%ACUMATICA_VERSION%"=="25R2" SET SITE_FOLDER=CFM-2025-R2

REM Add new versions here as needed:
REM IF /I "%ACUMATICA_VERSION%"=="26R1" SET SITE_FOLDER=CFM-2026-R1

IF "%SITE_FOLDER%"=="" (
    echo ERROR: Unsupported version "%ACUMATICA_VERSION%". Allowed: 23R2, 24R1, 24R2, 25R1, 25R2
    exit /b 65
)

REM Target deployment path
SET ACUMATICA_SITE_PATH=%SITES_ROOT%\%SITE_FOLDER%\Pages\%PAGE_FOLDER%

REM ========================================
REM Validation and Setup
REM ========================================

echo ========================================
echo Updating ATPT Site Pages (%ACUMATICA_VERSION%)
echo ========================================
echo Source: %SOURCE_PAGES_PATH%
echo Target: %ACUMATICA_SITE_PATH%

REM Check if source directory exists
if not exist "%SOURCE_PAGES_PATH%" (
    echo ERROR: Source directory does not exist: %SOURCE_PAGES_PATH%
    echo Please verify SOURCE_PAGES_PATH variable
    pause
    exit /b 1
)

REM Check if target directory exists, create if not
if not exist "%ACUMATICA_SITE_PATH%" (
    echo ERROR: Target directory does not exist.
    pause
    exit /b 1
)

REM Backup removed as per request – copying will be incremental only (new/modified files)

REM ========================================
REM Copy Process
REM ========================================

echo.
echo ========================================
echo Copying ASPX pages (only new or modified files)...
echo ========================================
echo Using ROBOCOPY for incremental copy...
echo.

REM Copy only new or modified files using robocopy
REM /XO -> exclude older source files (do not overwrite newer destination)
REM /FFT -> assume FAT file times (2-second granularity) to avoid false mismatches
REM /R:1 /W:1 -> retry once, wait 1s
REM Patterns: *.aspx and *.cs
robocopy "%SOURCE_PAGES_PATH%" "%ACUMATICA_SITE_PATH%" *.aspx *.cs /XO /FFT /R:0 /W:0

REM Interpret robocopy exit code: 0-7 are success/OK states; >=8 indicates failure
IF %ERRORLEVEL% GEQ 8 (
    echo ERROR: File copy failed, robocopy errorlevel %ERRORLEVEL%
    goto :RobocopyFailure
)

echo File copy completed successfully (robocopy errorlevel %ERRORLEVEL%)

REM ========================================
REM Verification
REM ========================================

echo ========================================
echo Copy step complete (incremental).
echo ========================================

REM ========================================
REM Summary
REM ========================================

echo.
echo ========================================
echo Update completed successfully!
echo ========================================
echo.
echo Summary:
echo   Source: %SOURCE_PAGES_PATH%
echo   Target: %ACUMATICA_SITE_PATH%
echo   Mode: Incremental (newer or missing files only)
echo   Acumatica Version: %ACUMATICA_VERSION%
echo   Page Folder: %PAGE_FOLDER%

echo.
echo IMPORTANT^: 
echo   - Restart IIS or recycle the application pool if needed
echo   - Test the pages in the Acumatica application
echo   - Verify all customizations are working correctly
echo.
exit /b 0

:RobocopyFailure
echo.
echo Script execution finished with errors.
exit /b 1
echo.

pause
endlocal