namespace Design.Models

open Shared.Enums
open Shared.Primitives
open System

(* Данные используемые для генерации событий *)

type SettingValues = 
    { RegistrationDate: DateTime
      BusinessFormType: BusinessFormType
      TaxationSystemTypes: seq<TaxationSystemType * Year> }
    with
      static member Default =
        { RegistrationDate = DateTime.MinValue 
          BusinessFormType = BusinessFormType.IP
          TaxationSystemTypes = Seq.empty }    

type Settings = 
    { FirmId : int64
      Values : SettingValues }
