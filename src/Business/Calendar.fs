namespace Business

open FSharp.Core.Fluent

open Design.Models.Calendar.Event
open Design.Models
open Mappers.Tax.Period

module Calendar =    

    let private rebuildEvents (setting : Setting.T) = async {
        let getRestrictions (period: Tax.Period) = (period, Restrictions.get period.TaxId |> Option.defaultValue [])
        let restrictionsFilter (period: Tax.Period, restrictions) = Restrictions.filter setting.Values restrictions period

        let! taxPeriods = DataAccess.Queries.Taxes.Periods.GetAll() 
        return taxPeriods.map(getRestrictions)
                         .filter(restrictionsFilter)
                         .map(fun (period, _) -> period, toEvent setting.FirmId period)
    }

    let private getExistingEvents (firmId : int) = async {
        let entityTypeIsTaxPeriod event = event.EntityType = EntityType.TaxPeriod

        let! existingEvents = DataAccess.Queries.Events.GetAllByFirmId firmId
        let  periodIds = existingEvents.filter(entityTypeIsTaxPeriod).map(fun event -> event.EntityId)
        let! periods = DataAccess.Queries.Taxes.Periods.GetAllByIds periodIds

        return existingEvents.filter(entityTypeIsTaxPeriod)
                             .map(fun event -> periods.find (fun period -> event.EntityId = period.Id), event)
    }

    let private mergeEvents
        (builtEvents    : (Tax.Period * Calendar.Event.T) seq)
        (existingEvents : (Tax.Period * Calendar.Event.T) seq) =

        let toRemove = existingEvents.filter(fun (eEventPeriod, _) -> 
                        builtEvents.exists(fun (bEventPeriod, _) -> eEventPeriod = bEventPeriod) |> not)
                                   .filter(fun (_, eEvent) -> eEvent.State <> State.Completed)
                                   .map(fun (_, eEvent) -> eEvent.Id)

        let toAdd = builtEvents.filter(fun (bEventPeriod, _) ->
                        existingEvents.exists(fun (eEventPeriod, _) -> eEventPeriod = bEventPeriod) |> not)
                                      .map(fun (_, bEvent) -> bEvent)

        (toRemove, toAdd)

    let rebuild (setting: Setting.T) = async {
        let! builtEvents    = rebuildEvents setting
        let! existingEvents = getExistingEvents setting.FirmId
        let  toRemove, toAdd   = mergeEvents builtEvents existingEvents

        return toRemove, toAdd
    }