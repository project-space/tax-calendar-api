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
                    then SettingValues.Default
                    else JsonConvert.DeserializeObject<SettingValues>(serializedValues)

            return
                { FirmId = firmId 
                  Values = desirealizedValue }
        }
        
        let public Save (settings: Settings) = 
            let serializedValues = JsonConvert.SerializeObject(settings.Values)
            let script = Core.ResourceManager.Get "DataAccess.Scripts.Settings_Save.sql"
            let param = 
                dict [
                    "FirmId" => settings.FirmId
                    "Values" => serializedValues
                ]

            ExecuteAsync script param