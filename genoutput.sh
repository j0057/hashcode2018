#!/bin/bash

for x in HashCode2018/inputs/*.in; do
    HashCode2018/bin/Debug/HashCode2018.exe $x > $(basename ${x/.in/.out})
done |& tee scores.txt
echo -n '--> '
awk '{s+=$1}END{print s}' scores.txt
git archive -o TeamKnuckle.zip master

