namespace DataAccess

open Core.Query
open Core.TVP
open Design.Models
open Newtonsoft.Json

module Queries =
    
    type Identificators =
        { Value : int }
        with
            static member Create identificators =
                identificators |> Seq.map (fun id -> { Value = id })

    module Settings =
        let Get (firmId: int) = async {
            let options = {
                Options.Default with
                 Script = Core.ResourceManager.Get "DataAccess.Scripts.Settings_Get.sql"
                 Parameters = dict [ "FirmId" => firmId ] }

            let! serializedValues = querySingleAsync options
            let desirealizedValue = 
                if isNull serializedValues 
                    then Setting.Values.Default
                    else JsonConvert.DeserializeObject<Setting.Values>(serializedValues)

            return
                { Setting.T.FirmId = firmId
                  Setting.T.Values = desirealizedValue }
        }
        
        let Save (setting: Setting.T) = 
            let serializedValues = JsonConvert.SerializeObject(setting.Values)
            let options = { 
                Options.Default with
                 Script = Core.ResourceManager.Get "DataAccess.Scripts.Settings_Save.sql" 
                 Parameters = 
                    dict [
                        "FirmId" => setting.FirmId
                        "Values" => serializedValues]} 

            executeAsync options

    module Taxes =
        module Periods =
            let GetAll() = 
                queryAsync<Tax.Period> { Options.Default with Script = Core.ResourceManager.Get "DataAccess.Scripts.Tax_Period_GetAll.sql" }

            let GetAllByIds (ids : int seq) =
                queryAsync<Tax.Period> 
                    { Options.Default with 
                       Script = Core.ResourceManager.Get "DataAccess.Scripts.Tax_Period_GetAllByIds.sql"
                       TVP = Some (create "Identificators" (Identificators.Create ids)) }

    module Events =
        let GetAllByFirmId (firmId: int) =
            queryAsync<Calendar.Event.T> 
                { Options.Default with 
                   Script = Core.ResourceManager.Get "DataAccess.Scripts.Calendar_Event_GetAllByFirmId.sql" 
                   Parameters = dict ["FirmId" => firmId ]}

        let Save (event: Calendar.Event.T) =
            executeAsync 
                { Options.Default with 
                   Script = Core.ResourceManager.Get "DataAccess.Scripts.Calendar_Event_Save.sql"
                   Parameters = 
                    dict [
                        "Id" => event.Id
                        "FirmId" => event.FirmId
                        "EntityId" => event.EntityId
                        "EntityType" => event.EntityType
                        "State" => event.State
                        "Start" => event.Start
                        "End" => event.End
                    ] }

        let RemoveByIds (ids: int seq) =
            executeAsync 
                { Options.Default with 
                   Script = Core.ResourceManager.Get "DataAccess.Scripts.Calendar_Event_RemoveByIds.sql"
                   TVP = Some (create "Identificators" (Identificators.Create ids)) }