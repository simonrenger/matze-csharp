#!/bin/bash

# help https://ryanstutorials.net/bash-scripting-tutorial/bash-arithmetic.php
tput clear
echo "#######################################"
echo "# Matze Maze Generator Example Script #"
echo "#######################################"
echo ""
if ! [ -x "$(command -v dotnet)" ]; then
  echo 'Error: dotnet is not installed.' >&2
  exit 1
fi
if ! [ -x "$(command -v bc)" ]; then
  echo 'Error: bc is not installed.' >&2
  exit 1
fi

if [ $# -eq 0 ]
  then
    echo "Error: No arguments supplied"
    echo "... [amount of runs] ([Algorithm=KR] [WIDTH=10] [HEIGHT=10])"
    exit 1
fi

if [[ $1 != ?(-)+([0-9]) ]]
  then
    echo "Error: The supplied argument [amount of runs] is not a number"
    echo "... [amount of runs] ([Algorithm=KR] [WIDTH=10] [HEIGHT=10])"
  exit 1
fi

ALGORITHM="KR"
WIDTH=10
HEIGHT=10

if [ $# > 2 ]
  then
  ALGORITHM=$2
fi

if [ $# -eq 3 ]
  then
    echo "Error: If the width is provided the Hight need to be provieded"
    echo "... [amount of runs] ([Algorithm=KR] [WIDTH=10] [HEIGHT=10])"
    exit 1
fi
echo "You want to generate $1 mazes";
echo -n "["
for ((i = 1; i <= $1; i++));
  do 
    let VAR="$i+1"
    RAW_PERCENT=$(bc <<< "scale=2;$i/$1")
    PERC=$(bc <<< "scale=2;$RAW_PERCENT*100.0")
    until dotnet run $ALGORITHM $WIDTH $HEIGHT &>/dev/null; do
        if [ $? -eq 1 ]; then
         echo -n ""
        else
            tput clear
            echo "Error: The Maze Generator had an error."
            exit 1;
        fi
        # potentially, other code follows...
    done
    tput cup 5 $VAR;
    echo -n "# ]";
    echo -n " $PERC%"
 done
 echo
 echo " Done"