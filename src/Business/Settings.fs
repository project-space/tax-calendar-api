namespace Business

module Settings =
    open DataAccess.Queries
    open Design.Models.Setting
    open DTO.Settings
    open Shared.Primitives
    
    let private applyChanges change values =
        match change with
        | Register (businessForm, taxationSystem) -> 
            { values with
                BusinessFormType = businessForm;
                TaxationSystemTypes = Seq.append values.TaxationSystemTypes [(taxationSystem, Year())]}

        | ChangeRegistrationDate (registrationDate) -> 
            { values with RegistrationDate = registrationDate }

        | ChangeTaxationSystem (year, taxationSystem) -> 
            let taxationSystemTypes =
                values.TaxationSystemTypes
                |> Seq.filter (snd >> (fun existingYear -> existingYear <> year ))
                |> Seq.append [(taxationSystem, year)]
                |> Seq.sortBy (snd)

            { values with TaxationSystemTypes = taxationSystemTypes }
 
    let public ChangeSettings (firmId: int64) (change: ChangeRequest) = async {
        let! settings        = Settings.Get firmId
        let  changedSettings = { settings with Values = applyChanges change settings.Values }
        let! _               = Settings.Save changedSettings

        return! Calendar.OnSettingsChanged changedSettings      
    }