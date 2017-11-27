namespace DataAccess

open Core.Query
open Design.Models.Settings
open Newtonsoft.Json

module Queries =

    module Settings =

        let public Get (firmId: int64) (year: int16) = async {
            let script = @"
                select
                    [Values] 
                from
                    YearSetting 
                where 
                    FirmId = @FirmId and 
                    Year = @Year"

            let param =
                dict [
                    "FirmId" => firmId
                    "Year" => year
                ]

            let! serializedValues = QuerySingleAsync script param
            let desirealizedValue = JsonConvert.DeserializeObject<YearSettingsValues>(serializedValues)

            return { FirmId = firmId; Year = year; Values = desirealizedValue }
        }


        let public Save (settings: YearSettings) = async {
            let script = @"
                if exists (select 1 from YearSetting where FirmId = @FirmId and Year = @Year)
                begin
                    update YearSetting set
                        [Values] = @Values
                end
                else begin
                    insert into YearSetting
                        values (@FirmId, @Year, @Values)
                end"

            let serializedValues = JsonConvert.SerializeObject(settings.Values)
            let param = 
                dict [
                    "FirmId" => settings.FirmId
                    "Year" => settings.Year
                    "Values" => serializedValues
                ]

            return! ExecuteAsync script param
        }