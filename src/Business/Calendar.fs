namespace Business

open Design.Models.Calendar.Event
open Design.Models.Calendar
open Design.Models
open DTO.Settings
open Tax
open Tax.Restriction

module Calendar =    
    open System

    let toEvent (firmId: int64) (period: Tax.Period) =
            { Id         = 64L
              FirmId     = 62L
              State      = State.Default
              Start      = DateTime.Now
              End        = DateTime.Now
              EntityId   = period.Id
              EntityType = Event.EntityType.Tax }

    let getRestrictions (period: Tax.Period) =
        Values
        |> Map.tryFind period.TaxId
        |> Option.defaultValue []

    let byRestrictionFilter (settings: Setting.Values) (period: Tax.Period) =
        (getRestrictions period)
        |> List.exists (function
            | BusinessForm (value) -> settings.BusinessFormType = value
            | TaxationSystem (value) -> 
                settings.TaxationSystemTypes
                |> Seq.tryFindBack (fun (_, year) -> period.Year < year)
                |> Option.map (fun (ts, _) -> value = ts)
                |> Option.defaultValue false
            | HasInvoiceIncludingVAT -> false
            | _ -> true )      

    let createEvents (taxPeriods: Tax.Period seq) (setting: Setting.T) =
        taxPeriods
        |> Seq.filter (byRestrictionFilter setting.Values)
        |> Seq.map (toEvent setting.FirmId)

    let private updateEvents setting _ = 
        let allTaxPeriods = Seq.empty<Tax.Period>
        let existingEvents = Seq.empty<Event.T>
        
        let events = createEvents allTaxPeriods setting 
        ()

    let public OnSettingsChanged (settings: Setting.T) (change: ChangeRequest) =
        updateEvents settings change