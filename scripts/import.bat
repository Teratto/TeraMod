@echo off
echo =====================================================
echo = IMPORTING STORY LINES                             =
echo =====================================================
python process_story.py --pull
if errorlevel 1 (
	goto fail
)

echo =====================================================
echo = IMPORTING STORY NODES                             =
echo =====================================================
python process_nodes.py --pull

echo =====================================================
echo = IMPORT SUCCESSFUL!! YAY!!                         =
echo =====================================================
pause
exit /b 0

:fail
echo !!!
echo !!! AN ERROR OCCURED - YOUR STORY WAS NOT IMPORTED!!
echo !!!
pause
exit /b 1
