set WORKSPACE=.
set LUBAN_DLL= excel2json\Luban\Luban.dll
set CONF_ROOT= excel2json

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=%WORKSPACE%\Jsons\

pause