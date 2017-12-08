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

    let private createEvents (taxPeriods: Tax.Period seq) (setting: Setting.T) =
        taxPeriods
        |> Seq.map (fun period -> (period, period.Restrictions))
        |> Seq.filter (fun (period, restrictions) -> Restrictions.filter setting.Values restrictions period)
        |> Seq.map (fun (period, _) -> toEvent setting.FirmId period)

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
        |> Seq.map (DataAccess.Queries.Events.Save >> Async.StartAsTask)
        |> System.Threading.Tasks.Task.WhenAll
        |> ignore

        return difference
    }

    let public OnSettingsChanged (settings: Setting.T) =
        updateEvents settings