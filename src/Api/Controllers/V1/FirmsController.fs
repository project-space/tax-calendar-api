namespace Api.Controllers

module Firms =
    open Design.Models.Setting
    open Giraffe.HttpHandlers
    open Giraffe.HttpContextExtensions
    open Giraffe.Tasks
    open Microsoft.AspNetCore.Http

    let public ChangeSettings (firmId: int) =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            let! request = context.BindJsonAsync<ChangeRequest>()
            let! result  = Business.Settings.change firmId request

            return! json result next context
        }

    let public GetAllEvents (firmId: int) =
        fun (next: HttpFunc) (context: HttpContext) -> task {
            return! next context
        }