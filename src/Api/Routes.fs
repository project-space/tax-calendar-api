namespace Api 

module Routes =
    open Giraffe.HttpHandlers
    open Controllers.Taxes
    open Controllers.Firms

    let private ``Firm Api version 1`` =
        subRouteCi "/Firms" 
            (choose [
                POST >=> routef "/%i/Settings/Change" ChangeSettings
                GET  >=> routef "/%i/Events/"         GetAllEvents
            ])
    
    let private ``Taxes Api version 1`` =
        subRouteCi "/Taxes"
            (choose [                
                POST >=> route  "/"   >=> PostSingle
                GET  >=> routef "/%i"     GetSingle
            ])

    let Default: HttpHandler = 
        subRouteCi "/api/v1" 
            (choose [
                ``Firm Api version 1``
                ``Taxes Api version 1``
            ])