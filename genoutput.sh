#!/bin/bash

for x in HashCode2018/inputs/*.in; do
    HashCode2018/bin/Debug/HashCode2018.exe $x > $(basename ${x/.in/.out})
done
git archive -o TeamKnuckle.zip master
