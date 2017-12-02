namespace DTO

module Settings =
    open System
    open Shared.Enums
    open Shared.Primitives

    type ChangeRequest =
        | Register of 
            BusinessForm: BusinessFormType *
            TaxationSystem: TaxationSystemType

        | ChangeRegistrationDate of
            RegistrationDate: DateTime

        | ChangeTaxationSystem of
            Year: Year *
            TaxationSystem: TaxationSystemType