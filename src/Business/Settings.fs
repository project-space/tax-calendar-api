namespace Business

open FSharp.Core.Fluent

module Settings =
    open DataAccess.Queries
    open Design.Models.Setting
    open Design.Enums
    
    let private applyChanges change values =
        match change with
        | Register (businessForm, taxationSystem) -> 
            { values with
                BusinessFormType = businessForm;
                TaxationSystemTypes = values.TaxationSystemTypes.append [(taxationSystem, 0)]}

        | ChangeRegistrationDate (registrationDate) -> 
            { values with 
                RegistrationDate = registrationDate }

        | ChangeTaxationSystem (year, taxationSystem) -> 
            { values with 
                TaxationSystemTypes = values.TaxationSystemTypes
                                            .filter(fun (_, _year) -> _year <> year)
                                            .append([(taxationSystem, year)])
                                            .sortBy(fun (_, _year) -> _year) }
 
    let change (firmId: int) (req: ChangeRequest) = async {
        let! settings        = Settings.Get firmId
        let  changedSettings = { settings with Values = applyChanges req settings.Values }
        let! _               = Settings.Save changedSettings

        return! Calendar.rebuild changedSettings      
    }