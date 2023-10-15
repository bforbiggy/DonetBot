# Donet Bot

My personal bot for adding features I feel like I 
A) actually want
B) need 
C) can code
D) don't want to add a bot for
E) will use.

## Environment setup (.env)

```
TOKEN=YOURBOTTOKENHERE
MONGODB_PASSWORD=MONGODBPASSWORDHERE
```

## Building for production/single file app

Find your runtime identifier (linux-x64)
https://learn.microsoft.com/en-us/dotnet/core/rid-catalog

```
dotnet build --configuration release
dotnet publish
```
