# Donet Bot

My personal bot for adding whatever features I feel like I actually want and need and can code and don't need a bot for and will use.

## Environment setup (.env)

```
TOKEN=YOURBOTTOKENHERE
MONGODB_PASSWORD=MONGODBPASSWORDHERE
```

## Building for production

```
dotnet build --configuration release
dotnet publish -r linux-x64 --configuration release
```
