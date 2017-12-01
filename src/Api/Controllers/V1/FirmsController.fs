namespace Api.Controllers

module Firms =
    open Giraffe.HttpHandlers
    open Giraffe.Tasks
    open Microsoft.AspNetCore.Http

    let public ChangeSettings (firmId: int64) =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            return! next context
        }

    let public GetAllEvents (firmId: int64) =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            return! next context
        }