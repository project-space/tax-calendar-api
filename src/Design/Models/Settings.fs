namespace Design.Models

module Setting =
  open Shared.Enums
  open Shared.Primitives
  open System

(* Данные используемые для генерации событий *)

  type Values = 
      { RegistrationDate: DateTime
        BusinessFormType: BusinessFormType
        TaxationSystemTypes: seq<TaxationSystemType * Year> }
      with
        static member Default =
          { RegistrationDate = DateTime.MinValue 
            BusinessFormType = BusinessFormType.IP
            TaxationSystemTypes = Seq.empty }    

  type T = 
      { FirmId : int64
        Values : Values }
