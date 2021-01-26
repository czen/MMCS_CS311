#FROM gitpod/workspace-dotnet
FROM ubuntu:latest

RUN apt-get update &&  apt install -y nuget mono-devel mono-xbuild
RUN nuget update -self
RUN mkdir /compilers

WORKDIR /compilers