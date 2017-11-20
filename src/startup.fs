module TaxCalendar.Api.Startup

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Logging
open Giraffe.HttpHandlers
open Giraffe.Middleware

open TaxCalendar.Api.Models

//---------------------------------------------------------------//
// app routing
//---------------------------------------------------------------//

let app =
    choose [
        GET >=>
            choose [
                route "/" >=> text "no content"
            ]

        setStatusCode 404 >=> text "not found"        
    ]

//---------------------------------------------------------------//
// error handling
//---------------------------------------------------------------//

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

//---------------------------------------------------------------//
// configs
//---------------------------------------------------------------//

let configureApp (appBuilder: IApplicationBuilder) =
    appBuilder
        .UseGiraffeErrorHandler(errorHandler)
        .UseGiraffe(app)

let configureServices services =
    ()

let configureLogging (builder: ILoggingBuilder) =
    let filter (level: LogLevel) = level >= LogLevel.Warning
    
    builder
        .AddFilter(filter)
        .AddConsole(fun o -> o.IncludeScopes <- true)
        .AddDebug() |> ignore