namespace Business
    module Restrictions = 
        open Design.Models
        open Tax.Restriction

        let filter 
            (settings     : Setting.Values)
            (restrictions : Tag list) 
            (period       : Tax.Period) =           

            List.exists (
                function
                | BusinessForm (value) -> settings.BusinessFormType = value
                | TaxationSystem (value) -> 
                    settings.TaxationSystemTypes
                    |> Seq.sortBy (fun (_, year) -> year)
                    |> Seq.tryFindBack (fun (_, year) -> year <= period.Year)
                    |> Option.map (fun (ts, _) -> value = ts)
                    |> Option.defaultValue false
                | HasInvoiceIncludingVAT -> false
            ) restrictions