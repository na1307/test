#!/usr/bin/bash

dnvmPath=~/.dnvm

mkdir $dnvmPath

cp ./dnvm $dnvmPath

cd $dnvmPath

curl -O -L https://dot.net/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh

mainShell=${SHELL##*/}

if [ $mainShell == "bash" ]; then
    echo "PATH=~/.dnvm:\$PATH" >> ~/.bashrc
elif [ $mainShell == "zsh" ]; then
    echo "PATH=~/.dnvm:\$PATH" >> ~/.zshrc
fi

mkdir instances

echo Installation Completed.
