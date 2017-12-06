namespace Business

open Design.Models.Calendar.Event
open Design.Models.Calendar
open Design.Models
open DTO.Settings
open Tax
open Tax.Restriction

module Calendar =    

    let private toEvent (firmId: int64) (period: Tax.Period) =
        let template = 
            { Id         = 0L
              FirmId     = firmId
              State      = State.Default
              Start      = period.Start
              End        = period.End
              EntityId   = period.Id
              EntityType = EntityType.TaxPeriod }

        template          

    let private byRestrictionFilter (settings: Setting.Values) (period: Tax.Period) =
        period.Restrictions
        |> List.exists (function
            | BusinessForm (value) -> settings.BusinessFormType = value
            | TaxationSystem (value) -> 
                settings.TaxationSystemTypes
                |> Seq.sortBy (fun (_, year) -> year)
                |> Seq.tryFindBack (fun (_, year) -> year <= period.Year)
                |> Option.map (fun (ts, _) -> value = ts)
                |> Option.defaultValue false
            | HasInvoiceIncludingVAT -> false
            | _ -> true )      

    let private createEvents (taxPeriods: Tax.Period seq) (setting: Setting.T) =
        taxPeriods
        |> Seq.filter (byRestrictionFilter setting.Values)
        |> Seq.map (toEvent setting.FirmId)

    let private updateEvents (setting: Setting.T) = async {
        let! allTaxPeriods = DataAccess.Queries.Taxes.Periods.GetAll() 
        let! existingEvents = DataAccess.Queries.Events.GetAllByFirmId(setting.FirmId)
        let createdEvents = createEvents allTaxPeriods setting

        let! _ = 
            existingEvents
            |> Seq.filter (fun e -> e.State <> State.Completed)
            |> Seq.map (fun e -> e.Id)
            |> DataAccess.Queries.Events.RemoveByIds

        let difference = 
            [existingEvents; createdEvents] 
            |> Seq.concat
            |> Seq.distinctBy (fun e -> e.EntityId, e.EntityType, e.Start, e.End)

        difference
        |> Seq.map (DataAccess.Queries.Events.Save)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore

        return difference
    }

    let public OnSettingsChanged (settings: Setting.T) =
        updateEvents settings