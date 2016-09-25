#!/bin/bash

lua="./../sprotodump/lua"
lpeg="./../sprotodump/lpeg.so"

if [[ ! -d "./../../skynet" ]]; then
	#statements
	git submodule update --init
fi

if [[ ! -f "$lua" ]]; then
	#statements	
	if [[ ! -f "./../../skynet/3rd/lua/lua" ]]; then
		#statements
		make -C ./../../skynet/3rd/lua linux
	fi
	cp ./../../skynet/3rd/lua/lua $lua
fi

if [[ ! -f "$lepg" ]]; then
	#statements
	if [[ ! -f "./../../skynet/3rd/lpeg/lpeg.so" ]]; then
		#statements
		make -C ./../../skynet/3rd/lpeg
	fi
	cp ./../../skynet/3rd/lpeg/lpeg.so $lpeg
fi
# $0 == test.sh
$lua util.lua
