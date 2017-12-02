namespace Api 

module Routes =
    open Giraffe.HttpHandlers
    open Controllers.Taxes
    open Controllers.Firms

    let private ``Firm Api version 1`` =
        subRouteCi "/Firms" 
            (choose [
                POST >=> routef "/%i/Settings/Change" ChangeSettings (* Изменяет данные фирмы, при этом добавляются или удаляются налоговые события *)
                GET  >=> routef "/%i/Events/"         GetAllEvents   (* Возвращает список налоговых событий для фирмы *)
            ])
    
    let private ``Taxes Api version 1`` =
        subRouteCi "/Taxes"
            (choose [
            ])

    let Default: HttpHandler = 
        subRouteCi "/api/v1" 
            (choose [
                ``Firm Api version 1``
                ``Taxes Api version 1``
            ])