namespace Business

open Design.Models.Calendar.Event
open Design.Models
open Mappers.Tax.Period

module Calendar =    

    let private rebuildEvents (setting : Setting.T) = async {
        let getRestrictions (period: Tax.Period) = (period, Restrictions.get period.TaxId |> Option.defaultValue [])
        let restrictionsFilter (period: Tax.Period, restrictions) = Restrictions.filter setting.Values restrictions period

        let! taxPeriods = DataAccess.Queries.Taxes.Periods.GetAll() 
        return
            taxPeriods
            |> Seq.map getRestrictions
            |> Seq.filter restrictionsFilter
            |> Seq.map (fun (period, _) -> period, ``to event`` setting.FirmId period)
    }

    let private getExistingEvents (firmId : int64) = async {
        let! existingEvents = DataAccess.Queries.Events.GetAllByFirmId firmId
        let periodIds = 
            existingEvents
            |> Seq.filter (fun event -> event.EntityType = EntityType.TaxPeriod)
            |> Seq.map (fun event -> event.EntityId)

        let! periods = DataAccess.Queries.Taxes.Periods.GetAllByIds periodIds

        return existingEvents
        |> Seq.filter (fun event -> event.EntityType = EntityType.TaxPeriod)
        |> Seq.map (fun event -> periods |> Seq.find (fun period -> event.EntityId = period.Id), event)
    }

    let private mergeEvents
        (builtEvents    : (Tax.Period * Calendar.Event.T) seq)
        (existingEvents : (Tax.Period * Calendar.Event.T) seq) =

        let toRemove =
            existingEvents  |> Seq.filter (fun (eEventPeriod, _) ->
                builtEvents |> Seq.exists (fun (bEventPeriod, _) -> eEventPeriod = bEventPeriod) |> not)
                                 |> Seq.filter (fun (_, eEvent) -> eEvent.State <> State.Completed)
                                 |> Seq.map    (fun (_, eEvent) -> eEvent.Id)

        let toAdd =
            builtEvents        |> Seq.filter (fun (bEventPeriod, _) ->
                existingEvents |> Seq.exists (fun (eEventPeriod, _) -> eEventPeriod = bEventPeriod) |> not)
                                    |> Seq.map    (fun (_, bEvent) -> bEvent)

        (toRemove, toAdd)

    let rebuild (setting: Setting.T) = async {
        let! builtEvents    = rebuildEvents setting
        let! existingEvents = getExistingEvents setting.FirmId
        let  toRemove, toAdd   = mergeEvents builtEvents existingEvents

        return toRemove, toAdd
    }