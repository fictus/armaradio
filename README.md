A radio project aimed at showcasing my experience as a Software Developer.
This project was created for portfolio purposes only! Please use responsibly



IMPORTANT: I have omitted all appsettings.json files to protect sensitive data
YOU MUST create your own appsettings.json files

ARMARADIO appsettings.json must look like this:

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
