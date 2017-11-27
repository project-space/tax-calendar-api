namespace Api 

module Routes =
    open Giraffe.HttpHandlers
    open Controllers.Taxes

    let private ``Firm API V1`` =
        subRouteCi "/Firms" 
            (choose [
                GET >=> route "/" >=> text "FIRMS API V1"
            ])

    let private ``Taxes API V1`` =
        subRouteCi "/Taxes"
            (choose [
                GET >=> route "/" >=> text "TAXES API V1"
                
                POST >=> routeCif "/%s/ValidityPeriod" (PostValidityPeriod >> json)
            ])

    let Default: HttpHandler = 
        subRouteCi "/api/v1" 
            (choose [
                ``Firm API V1``
                ``Taxes API V1``
            ])