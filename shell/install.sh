#!/usr/bin/bash

ndnmPath=~/.ndnm

mkdir $ndnmPath

cp ./ndnm $ndnmPath

cd $ndnmPath

curl -O -L https://dot.net/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh

mainShell=${SHELL##*/}

if [ $mainShell == "bash" ]; then
    echo "PATH=~/.ndnm:\$PATH" >> ~/.bashrc
elif [ $mainShell == "zsh" ]; then
    echo "PATH=~/.ndnm:\$PATH" >> ~/.zshrc
fi

mkdir instances

echo Installation Completed.
