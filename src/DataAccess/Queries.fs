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