# DotDispatch
This project is a simple web server using asp.net core 8.0 that is suited for static SPA apps.

It returns back an index.html file in a `wwwroot` folder for every request it receives.

If index.html file is not found, it returns a default string message.

The default content can be customized through a key named `DefaultContent` in `appsettings.json` file.

