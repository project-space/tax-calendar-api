namespace Api 

module Routes =
    open Giraffe.HttpHandlers
    open Controllers.Taxes

    let private ``Firm API V1`` =
        subRouteCi "/Firms" 
            (choose [
            ])

    let private ``Taxes API V1`` =
        subRouteCi "/Taxes"
            (choose [
                GET  >=> route  "/"   >=> Get
                POST >=> route  "/"   >=> PostSingle

                GET  >=> routef "/%i"     GetSingle
            ])

    let Default: HttpHandler = 
        subRouteCi "/api/v1" 
            (choose [
                ``Firm API V1``
                ``Taxes API V1``
            ])