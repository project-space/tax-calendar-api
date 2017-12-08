namespace Business
    module Restrictions = 
        open Design.Models
        open Shared.Enums

        (* Тэги, указывающие на признаки (aka Restrictions), при наличии которых (хотя бы одного) уплачивается налог *)      
        type Tag =
            | BusinessForm of BusinessFormType
            | TaxationSystem of TaxationSystemType
            | HasInvoiceIncludingVAT

        let private taxRestrictions = 
            [(Tax.Id.VAT, [
                TaxationSystem(TaxationSystemType.OSNO)
                HasInvoiceIncludingVAT])

             (Tax.Id.PersonalProperty, [
                BusinessForm(BusinessFormType.IP)])

             (Tax.Id.PersonalIncome, [
                BusinessForm(BusinessFormType.IP)])

            ] |> Map.ofList

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
                    |> Seq.sortBy (fun (_, year) -> year)
                    |> Seq.tryFindBack (fun (_, year) -> year <= period.Year)
                    |> Option.map (fun (ts, _) -> value = ts)
                    |> Option.defaultValue false
                | HasInvoiceIncludingVAT -> false
            ) restrictions