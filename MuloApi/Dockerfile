#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
LABEL maintainer="Disdimirov"

# Set current working directory api-playerMusicLover
WORKDIR /home/api-playerMusicLover

# All copying code from your local context to the image working directory
COPY . .

# Run app dotnetcore MuloApi.dll
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MuloApi.dll