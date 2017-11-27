namespace Api

open System
open Microsoft.Extensions.Logging
open Giraffe.HttpHandlers

module Handlers =

    let errorHandler (ex: Exception) (logger: ILogger) =
        logger.LogError(EventId(), ex, "Unhandled exception.")
        clearResponse >=> setStatusCode 500 >=> text ex.Message