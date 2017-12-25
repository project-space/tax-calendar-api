namespace Design.Models

open Design.Enums
open System

module Setting =

    (* Данные используемые для генерации событий *)
    type Values = 
        { RegistrationDate: DateTime
          BusinessFormType: BusinessFormType
          TaxationSystemTypes: seq<TaxationSystemType * int (* Year *) > }
    with
      static member Default =
        { RegistrationDate = DateTime.MinValue 
          BusinessFormType = BusinessFormType.IP
          TaxationSystemTypes = Seq.empty }    

    type T = 
        { FirmId : int
          Values : Values }
        with
          static member Default = 
            { FirmId = 0 
              Values = Values.Default }

    type ChangeRequest =
        | Register of 
            BusinessForm: BusinessFormType *
            TaxationSystem: TaxationSystemType

        | ChangeRegistrationDate of
            RegistrationDate: DateTime

        | ChangeTaxationSystem of
            Year: int *
            TaxationSystem: TaxationSystemType
