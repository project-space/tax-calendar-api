namespace Business

open FSharp.Core.Fluent

open Design.Enums
open Design.Models

module Restrictions = 

    (* Тэги, указывающие на признаки (aka Restrictions), при наличии которых (хотя бы одного) уплачивается налог *)      
    type Tag =
        | BusinessForm of BusinessFormType
        | TaxationSystem of TaxationSystemType
        | HasInvoiceIncludingVAT

    let private taxRestrictions = 
        Map.ofList <|
        [(Tax.Id.VAT,              [HasInvoiceIncludingVAT; TaxationSystem(TaxationSystemType.OSNO)])
         (Tax.Id.PersonalProperty, [BusinessForm(BusinessFormType.IP)])
         (Tax.Id.PersonalIncome,   [BusinessForm(BusinessFormType.IP)])]

    let get (taxId: Tax.Id) = taxRestrictions |> Map.tryFind taxId

    let filter 
        (settings     : Setting.Values)
        (restrictions : Tag list) 
        (period       : Tax.Period) =           

        List.exists (
            function
            | BusinessForm (value) -> settings.BusinessFormType = value
            | TaxationSystem (value) -> 
                settings.TaxationSystemTypes
                        .tryFindBack(fun (_, year) -> year <= period.Year)
                        |> Option.map (fun (ts, _) -> value = ts)
                        |> Option.defaultValue false

            | HasInvoiceIncludingVAT -> false
        ) restrictions