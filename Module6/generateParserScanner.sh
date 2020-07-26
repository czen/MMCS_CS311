#!/bin/bash
mono ../gplex/bin/Gplex.exe /unicode SimpleLex.lex
mono ../gppg/bin/Gppg.exe /no-lines /gplex SimpleYacc.y
