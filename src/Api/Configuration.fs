namespace Api

module Configuration =

    open Giraffe.Middleware
    open Handlers
    open Microsoft.AspNetCore.Builder
    open Microsoft.Extensions.DependencyInjection
    open Microsoft.Extensions.Logging
    open Routes

    let configureServices (_: IServiceCollection) =
        ()

    let configureLogging (builder: ILoggingBuilder) =
        builder
            .AddFilter(fun level -> level >= LogLevel.Warning)
            .AddConsole()
            .AddDebug() |> ignore

    let configureApp (app: IApplicationBuilder) =
        app
            .UseGiraffeErrorHandler(errorHandler)
            .UseGiraffe Default
       
    