namespace DataAccess

open Core.Query
open Design.Models
open Newtonsoft.Json

module Queries =
    module Settings =
        let public Get (firmId: int64) = async {
            let script = Core.ResourceManager.Get "DataAccess.Scripts.Settings_Get.sql"
            let param = dict [ "FirmId" => firmId ]

            let! serializedValues = QuerySingleAsync script param
            let desirealizedValue = 
                if isNull serializedValues 
                    then Setting.Values.Default
                    else JsonConvert.DeserializeObject<Setting.Values>(serializedValues)

            return
                { Setting.T.FirmId = firmId
                  Setting.T.Values = desirealizedValue }
        }
        
        let public Save (setting: Setting.T) = 
            let serializedValues = JsonConvert.SerializeObject(setting.Values)
            let script = Core.ResourceManager.Get "DataAccess.Scripts.Settings_Save.sql"
            let param = 
                dict [
                    "FirmId" => setting.FirmId
                    "Values" => serializedValues
                ]

            ExecuteAsync script param

    module Taxes =
        module Periods =
            let public GetAll() = 
                let script = Core.ResourceManager.Get "DataAccess.Scripts.Tax_Period_GetAll.sql"
                QueryAsync<Tax.Period> script null

    module Events =
        let public GetAllByFirmId (firmId: int64) =
            let script = Core.ResourceManager.Get "DataAccess.Scripts.Calendar_Event_GetAllByFirmId.sql"
            let param = dict ["FirmId" => firmId ]
            
            QueryAsync<Calendar.Event.T> script param

        let public Save (event: Calendar.Event.T) =
            let script = Core.ResourceManager.Get "DataAccess.Scripts.Calendar_Event_Save.sql"
            ExecuteAsync script event

        let public RemoveByIds (ids: int64 seq) =
            let script = "DataAccess.Scripts.Calendar_Event_RemoveByIds.sql"
            ExecuteAsync script ids        