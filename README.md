A radio project aimed at showcasing my experience as a Software Developer. This project was created for portfolio purposes only! Please use responsibly

**IMPORTANT**: I have omitted all appsettings.json files to protect sensitive data, **YOU MUST create your own** appsettings.json files

armaradio solution appsettings.json must look like this:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "radioconn": "XXXX",
    "adminconn": "XXXX",
    "recommendations": "XXXX"
  },
  "ApplicationConfiguration": {
    "apiClientId": "XXXX",
    "apiClientSecret": "XXXX"
  }
}
```

# INCLUDED PROJECTS

**armaradio** - An ASP.NET core 8 MVC website for playing music and creating playlists.

**arma_historycompiler** - A service/daemon for downloading the latest listenbrainz music listening history. The service then saves all listening history by user into SQL.

**arma_miner** - A service/daemon for downloading the latest artists & albums data from metabrainz. Results are stored into SQL.

**armaradio_ops** - A service/daemon I wrote for seeding the initial artists/albums dump into SQL.

**armaoffline** - The Android (and possibly iOS) mobile app for listening to your playlists offline withour requiring an internet connection.

**the other projects** - I was no longer using them at the end, so I can't really recall what their purpose were.
