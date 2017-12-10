namespace Business

open Design.Models.Calendar.Event
open Design.Models
open Mappers.Tax.Period

module Calendar =    

    let private ``rebuild events`` (setting : Setting.T) = async {
        let getRestrictions (period: Tax.Period) = (period, Restrictions.get period.TaxId |> Option.defaultValue [])
        let restrictionsFilter (period: Tax.Period, restrictions) = Restrictions.filter setting.Values restrictions period

        let! taxPeriods = DataAccess.Queries.Taxes.Periods.GetAll() 
        return
            taxPeriods
            |> Seq.map getRestrictions
            |> Seq.filter restrictionsFilter
            |> Seq.map (fun (period, _) -> period, ``to event`` setting.FirmId period)
    }

    let private ``get existing events`` (firmId : int64) = async {
        let! ``existing events`` = DataAccess.Queries.Events.GetAllByFirmId firmId
        let periodIds = 
            ``existing events``
            |> Seq.filter (fun event -> event.EntityType = EntityType.TaxPeriod)
            |> Seq.map (fun event -> event.EntityId)

        let! periods = DataAccess.Queries.Taxes.Periods.GetAllByIds periodIds

        return ``existing events``
        |> Seq.filter (fun event -> event.EntityType = EntityType.TaxPeriod)
        |> Seq.map (fun event -> periods |> Seq.find (fun period -> event.EntityId = period.Id), event)
    }

    let private ``merge events``
        (``built events``    : (Tax.Period * Calendar.Event.T) seq)
        (``existing events`` : (Tax.Period * Calendar.Event.T) seq) =

        let ``events to be removed`` =
            ``existing events``  |> Seq.filter (fun (eEventPeriod, _) ->
                ``built events`` |> Seq.exists (fun (bEventPeriod, _) -> eEventPeriod = bEventPeriod) |> not)
                                 |> Seq.filter (fun (_, eEvent) -> eEvent.State <> State.Completed)
                                 |> Seq.map    (fun (_, eEvent) -> eEvent.Id)

        let ``events to be added`` =
            ``built events``        |> Seq.filter (fun (bEventPeriod, _) ->
                ``existing events`` |> Seq.exists (fun (eEventPeriod, _) -> eEventPeriod = bEventPeriod) |> not)
                                    |> Seq.map    (fun (_, bEvent) -> bEvent)

        (``events to be removed``, ``events to be added``)

    let rebuild (setting: Setting.T) = async {
        let! ``built events``    = ``rebuild events`` setting
        let! ``existing events`` = ``get existing events`` setting.FirmId
        let  (toRemove, toAdd)   = ``merge events`` ``built events`` ``existing events``

        return (toRemove, toAdd)
    }