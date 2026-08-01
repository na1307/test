@echo off

cd /d %~dp0

xmake f -m debug
xmake package
xmake f -m release
xmake package
