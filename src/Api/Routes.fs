namespace Api 

module Routes =
    open Giraffe.HttpHandlers
    open DataAccess.Queries

    let Default: HttpHandler = 
        choose [
            GET >=> 
                choose [
                    route "/" >=> text "Hello, this is tax calendar API"
                    routef "/settings/%i/%i" (fun (year, firmId) ->
                            let (f, y) = (int64(firmId), int16(year)) 
                            let r = (Settings.Get f y) |> Async.RunSynchronously
                            json r 
                        )
                ]

            setStatusCode 404 >=> text "Not found"             
        ]