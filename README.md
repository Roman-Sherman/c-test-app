# aspnetcore-webapp-sample

## Preface
This sample project uses [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) and [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/). It provides only Web API. So, I recommend using a [vuejs-webapp-sample](https://github.com/ybkuroki/vuejs-webapp-sample) project as Web Front End.

## Install
Perform the following steps:

1. Download and install [.NET Core SDK](https://www.microsoft.com/net/download).
1. Download and install [Visual Studio Code(VS Code)](https://code.visualstudio.com/).
1. Install [C# for Visual Studio Code (powered by OmniSharp)](https://github.com/OmniSharp/omnisharp-vscode) extension for VS Code.
1. Clone this repository.
1. Opening the project folder will download dependencies.

## Starting Server
Perform the following steps:

1. Open this sample project.
1. Select **Debug > Start Debugging** in menu bar.
1. When startup is complete, the console shows the following message:
    ```
    Hosting environment: Development
    Content root path: [Project Path]
    Now listening on: http://[::]:8080
    Application started. Press Ctrl+C to shut down.
    ```
1. Access [http://localhost:8080/api/health](http://localhost:8080/api/health) in your browser and confirm that this application has started.

## Creating a Production Build
Perform the following command:

```bash
$ dotnet publish -c Release -o out
```

## Project Map
The follwing figure is the map of this sample project.

```
- aspnetcore-webapp-sample
  + Common                  … Provide a common service of this system.
  + Controllers             … Define controllers.
  + Models                  … Define models.
  + Repositories            … Provide a service of database access.
  + Services                … Provide a service of book management.
  + Logger                  … Provides logging using Log4Net
  - Startup.cs              … Define configurations such as database connections, security, and swagger.
  - Program.cs              … Entry Point.
  - log4net.config          … The configuration file for Log4Net
```

## Services
This sample provides 3 services: book management, account management, and master management. Web APIs of this sample can be confirmed from [Swagger](http://localhost:8080/swagger).

### Book Management
There are the following services in the book management.

|Service Name|HTTP Method|URL|Parameter|Summary|
|:---|:---:|:---|:---|:---|
|List Service|GET|``/api/book/list``|Page|Get a list of books.|
|Regist Service|POST|``/api/book/new``|Book|Regist a book data.|
|Edit Service|POST|``/api/book/edit``|Book|Edit a book data.|
|Delete Service|POST|``/api/book/delete``|Book|Delete a book data.|
|Search Title Service|GET|``/api/book/search``|Keyword, Page|Search a title with  the specified keyword.|
|Report Service(No implementation)|GET|``/api/book/allListPdfReport``|Nothing|Output a list of books to the PDF file.|

### Account Management
There are the following services in the Account management.

|Service Name|HTTP Method|URL|Parameter|Summary|
|:---|:---:|:---|:---|:---|
|Login Service|POST|``/api/account/login``|Session ID, User Name, Password|Session authentication with username and password.|
|Logout Service|POST|``/api/account/logout``|Session ID|Logout a user.|
|Login Status Check Service|GET|``/api/account/loginStatus``|Session ID|Check if the user is logged in.|
|Login Username Service|GET|``/api/account/loginAccount``|Session ID|Get the login user's username.|

### Master Management
There are the following services in the Master management.

|Service Name|HTTP Method|URL|Parameter|Summary|
|:---|:---:|:---|:---|:---|
|Category List Service|GET|``/api/master/category``|Nothing|Get a list of categories.|
|Format List Service|GET|``/api/master/format``|Nothing|Get a list of formats.|

## Libraries
This sample uses the following libraries.

|Library Name|Version|
|:---|:---:|
|ASP.NET Core|2.2|
|Entity Framework Core|2.2|
|System.Linq|4.3.0|
|Newtonsoft.Json|11.0.2|
|log4net|2.0.8|
|Swashbuckle.AspNetCore|3.0.0|

## License
The License of this sample is *MIT License*.
